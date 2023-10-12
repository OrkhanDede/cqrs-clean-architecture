using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace Logging
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly long _start;


        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _start = Stopwatch.GetTimestamp();
        }

        public async Task Invoke(HttpContext httpContext)
        {

            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Push the user name into the log context so that it is included in all log entries
            LogContext.PushProperty("UserId", userId);

            // Getting the request body is a little tricky because it's a stream
            // So, we need to read the stream and then rewind it back to the beginning

            await RequestLog(httpContext.Request).ConfigureAwait(false);
            // await _next(httpContext).ConfigureAwait(false);
            // The reponse body is also a stream so we need to:
            // - hold a reference to the original response body stream
            // - re-point the response body to a new memory stream
            // - read the response body after the request is handled into our memory stream
            // - copy the response in the memory stream out to the original response stream
            await ResponseLog(httpContext).ConfigureAwait(false);
        }

        private async Task RequestLog(HttpRequest request)
        {
            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();
            var body = request.Body;



            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;

            Log.ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), true)
                .ForContext("RequestBody", requestBody).Information("Request information {RequestMethod} {RequestPath}",
                    request.Method, request.Path + request.QueryString);
        }

        private async Task ResponseLog(HttpContext httpContext)
        {
            var response = httpContext.Response;


            var originalBody = httpContext.Response.Body;
            string responseBody = "";
            await using var memStream = new MemoryStream();

            try
            {

                response.Body = memStream;

                // try
                // {

                try
                {
                    await _next(httpContext).ConfigureAwait(false);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    var errorId = Guid.NewGuid();
                    Log.ForContext("Type", "Error").ForContext("Exception", ex, true)
                        .Error(ex, ex.Message + ". {@errorId}", errorId);
                    if (ex is ApiException exception)
                    {
                        response.StatusCode = exception.StatusCode;
                    }

                    throw;
                }
                if (response.Body.CanRead)
                {
                    response.Body.Seek(0, SeekOrigin.Begin);
                    responseBody = await new StreamReader(response.Body).ReadToEndAsync().ConfigureAwait(false);
                    response.Body.Seek(0, SeekOrigin.Begin);

                }



            }
            finally
            {

                LogContext.PushProperty("Type", "Response");
                Log.ForContext("ResponseHeaders", response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()))
                    .ForContext("ResponseBody", responseBody)
                    .Information("Response information {RequestMethod} {RequestPath} {statusCode} {ElapsedTime} s",
                        httpContext.Request.Method, httpContext.Request.Path + httpContext.Request.QueryString, response.StatusCode,
                        GetElapsedMilliseconds(_start, Stopwatch.GetTimestamp()));


                await memStream.CopyToAsync(originalBody).ConfigureAwait(false);

                httpContext.Response.Body = originalBody;
            }

        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (double)(((stop - start) * 1000L) / (double)Stopwatch.Frequency);
        }
    }
}
