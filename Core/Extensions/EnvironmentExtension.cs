using System;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions
{
    public  class EnvironmentExtension
    {
        public static readonly bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
        public static readonly bool IsStaging = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Staging;
        public static readonly bool IsProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production;
    }
}
