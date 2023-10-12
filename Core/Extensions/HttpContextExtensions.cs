using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Core.Extensions
{
    public static class HttpContextExtensions
    {

        public static T GetHeaderValueAs<T>(this HttpContext httpContext, string headerName)
        {
            StringValues values = new StringValues();

            if (httpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
        public static string GetRefreshToken(this HttpContext httpContext)
        {
            return httpContext.GetHeaderValueAs<string>("RefreshToken");
        }
        public static string GetAcceptLanguage(this HttpContext httpContext)
        {
            return httpContext.GetHeaderValueAs<string>("Accept-Language");
        }

        public static string GetRecaptchaToken(this HttpContext httpContext)
        {
            return httpContext.GetHeaderValueAs<string>("RecaptchaToken");
        }
        public static string GetAuthorizationHeader(this HttpContext httpContext)
        {
            return httpContext.GetHeaderValueAs<string>("Authorization");
        }
        public static string GetAuthorizationToken(this HttpContext httpContext)
        {
            return httpContext.GetAuthorizationHeader()?.Replace("Bearer", "").Trim();
        }

        public static string GetDomainUrl(this HttpContext httpContext)
        {
            Uri uri;
            //var uri = new Uri(httpContext.GetHeaderValueAs<string>("Origin"));
            //var domain = uri.Host;
            string domain = string.Empty;
            var origin = httpContext.GetHeaderValueAs<string>("Origin");
            var referrer = httpContext.GetHeaderValueAs<string>("Referer");
            var host = httpContext.GetHeaderValueAs<string>("Host");


            string from = "";
            if (Uri.TryCreate(origin, UriKind.Absolute, out uri))
            {
                from = "origin";
                domain = uri.Host;
            }
            else if (Uri.TryCreate(referrer, UriKind.Absolute, out uri))
            {
                from = "referrer";
                domain = uri.Host;
            }
            else if (!string.IsNullOrEmpty(host))
            {
                from = "host";
                domain = host;
            }

            //Log.ForContext("Type", "GetDomainUrl").ForContext("From", from).ForContext("Domain", domain).Information("Get Domain Url");

            return domain;
        }
        public static string GetRequestIp(this HttpContext httpContext, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = httpContext.GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrEmpty(ip) && httpContext?.Connection?.RemoteIpAddress != null)
                ip = httpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrEmpty(ip))
                ip = httpContext.GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            // if (string.IsNullOrEmpty(ip))
            //     throw new Exception("Unable to determine caller's IP.");

            return ip;
        }
    }
}
