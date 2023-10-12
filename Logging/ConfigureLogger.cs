using System;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.RollingFileAlternate;

namespace Logging
{
    public static class ConfigureLogger
    {
        public static LoggerConfiguration BuildLoggerConfiguration(this LoggerConfiguration loggerConfiguration)
        {

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;

            var logPath = isDevelopment ? "../Logging/Logs" : "Logs";

            var conf = loggerConfiguration.MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                //.MinimumLevel.Override("System.Net.Http.HttpClient.ProxyKitClient.LogicalHandler",
                //    LogEventLevel.Warning)
                //.MinimumLevel.Override("System.Net.Http.HttpClient.ProxyKitClient.ClientHandler", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo
                .RollingFileAlternate(new RenderedCompactJsonFormatter(), logPath, fileSizeLimitBytes: 314572800) //300mb
                .WriteTo.Console(LogEventLevel.Information);



            return conf;
        }
    }
}
