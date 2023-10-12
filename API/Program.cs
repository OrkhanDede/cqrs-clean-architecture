using System;
using System.Threading.Tasks;
using Data;
using DataAccess;
using DataAccess.Initialize;
using Domain.Entities.Identity;
using Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            Log.Logger = new LoggerConfiguration().BuildLoggerConfiguration().CreateLogger();

            try
            {
                Log.Information("Starting up");
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
                    await Initialize.
                        SeedAsync(context, userManager, roleManager).
                        ConfigureAwait(false);
                }
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                Log.CloseAndFlush();
            }
            finally
            {
                Log.Information("Started");
            }


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("Settings/tokenSettings.json", false, true);
                    config.AddJsonFile("Settings/aesCryptoSettings.json", false, true);
                    config.AddJsonFile("Settings/appsettings.json", false, true);
                    config.AddJsonFile($"Settings/appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    config.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
