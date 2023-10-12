using System;
using System.IO;
using System.Reflection;
using Application;
using Core.Models;
using Core.Resources;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLocalization();
            services.AddControllers().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(Resource));
            }).AddNewtonsoftJson();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // set Modelstate validation manual
            });
            services.Configure<TokenSettings>(Configuration.GetSection("tokenSettings"));
            services.Configure<AesCryptoSettings>(Configuration.GetSection("aesCryptoSettings"));

            services.ConfigureDependencyInjections(Configuration, new DependencyInjectionOptions()
            {
                AutoMapperAssemblies = new Assembly[]
                {
                    GetType().Assembly,
                    typeof(DependencyInjection).Assembly,
                    typeof(AuthService).Assembly,
                },
                SwaggerAssembly = Assembly.GetExecutingAssembly()
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {

            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";

            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "cqrs-base-architecture v1");
                c.RoutePrefix = "api/swagger";
            });
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthorization();
            app.ConfigureAutoWrapperMiddleware();
            app.ConfigureLoggingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
