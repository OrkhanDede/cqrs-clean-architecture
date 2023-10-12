using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using AutoWrapper;
using Core.Extensions;
using Core.Models;
using Data;
using DataAccess;
using DataAccess.Repository;
using Domain.Entities.Identity;
using Infrastructure.Middlewares;
using Infrastructure.Pipelines;
using Infrastructure.Services;
using Logging;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Application
{

    public class DependencyInjectionOptions
    {
        public Assembly[] AutoMapperAssemblies { get; set; }
        public Assembly SwaggerAssembly { get; set; }
    }

    public static class DependencyInjection
    {
        private static readonly bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
        public static IServiceCollection ConfigureDependencyInjections(this IServiceCollection services,
            IConfiguration configuration, DependencyInjectionOptions options)
        {
            services
                .ConfigureIdentity()
                .ConfigureAuthentication(configuration)
                .ConfigureAuthorization()
                .ConfigureDatabase(configuration)
                .ConfigureCors()
                .AddHttpClient()
                .AddRepositories()
                .ConfigureAutoMapper(options.AutoMapperAssemblies)
                .ConfigureServices()
                .AddSwagger(options.SwaggerAssembly)
                .AddMediator();
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services, Assembly assembly)
        {
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "cqrs-base-architecture",
                    Version = "v1",
                    Description = "ASP.NET Core 5 Web API"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);

            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
            return services;
        }
        public static IApplicationBuilder ConfigureAutoWrapperMiddleware(this IApplicationBuilder builder)
        {
            builder.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            {
                //UseApiProblemDetailsException = true,
                IgnoreNullValue = false,
                ShowStatusCode = true,
                ShowIsErrorFlagForSuccessfulResponse = true,
                //IgnoreWrapForOkRequests = true,
                IsDebug = IsDevelopment,

                EnableExceptionLogging = false,
                EnableResponseLogging = false,
                LogRequestDataOnException = false,
                ShouldLogRequestData = false,


                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                UseCustomExceptionFormat = false,
            });

            return builder;
        }
        public static IApplicationBuilder ConfigureLoggingMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<LoggingMiddleware>();
            // builder.UseMiddleware<RequestResponseLoggingMiddleware>();

            return builder;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // i assume your service interfaces inherit from IRepositoryBase<>
            Assembly ass = typeof(IRepositoryIdentifier).GetTypeInfo().Assembly;

            // get all concrete types which implements IRepositoryIdentifier
            var allRepositories = ass.GetTypes().Where(t =>
                t.GetTypeInfo().IsClass &&
                !t.IsGenericType &&
                !t.GetTypeInfo().IsAbstract &&
                typeof(IRepositoryIdentifier).IsAssignableFrom(t));

            foreach (var type in allRepositories)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Where(t => typeof(IRepositoryIdentifier) != t && (!t.IsGenericType || t.GetGenericTypeDefinition() != typeof(IRepository<>)));
                foreach (var itype in mainInterfaces)
                {
                    if (allRepositories.Any(x => x != type && itype.IsAssignableFrom(x)))
                    {
                        throw new Exception("The " + itype.Name + " type has more than one implementations, please change your filter");
                    }
                    services.AddScoped(itype, type);
                }
            }
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        private static IServiceCollection ConfigureAutoMapper(this IServiceCollection services, Assembly[] assemblies)
        {
            services.AddAutoMapper(assemblies);
            return services;
        }
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {

                var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
                var appDomains = context.AppDomains.ToList();
                bool IsOriginAllowed(string origin)
                {
                    Uri uri;
                    if (Uri.TryCreate(origin, UriKind.Absolute, out uri))
                    {
                        origin = uri.Host;
                    }

                    if (!string.IsNullOrEmpty(origin))
                    {
                        var isAllowed = appDomains.Any(c => origin.EndsWith(c.Domain, StringComparison.OrdinalIgnoreCase));

                        if (!isAllowed && !EnvironmentExtension.IsProduction)
                        {
                            //uri = new Uri(origin);
                            isAllowed = origin.Equals("localhost", StringComparison.OrdinalIgnoreCase);
                        }
                        return isAllowed;
                    }

                    return false;
                }
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("")
                        // .WithExposedHeaders("Upload-Offset", "Location", "Upload-Length", "Tus-Version", "Tus-Resumable", "Tus-Max-Size", "Tus-Extension", "Upload-Metadata", "Upload-Defer-Length", "Upload-Concat", "Location", "Upload-Offset", "Upload-Length")
                        .SetIsOriginAllowed(IsOriginAllowed);
                });

            });
            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<PdfService>();
            services.AddScoped<TokenService>();
            services.AddScoped<FileService>();
            services.AddScoped<AccessLogService>();
            services.AddScoped<SignInService>();
            services.AddScoped<TranslationService>();
            services.AddScoped<AccessLimitService>();
            services.AddScoped<ExceptionService>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            return services;
        }

        private static IServiceCollection ConfigureDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));

            return services;
        }
        private static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(
                options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 0;
                }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            return services;
        }
        private static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("TokenSettings");
            services.Configure<TokenSettings>(appSettingsSection);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var appSettings = appSettingsSection.Get<TokenSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.JwtKey);
            var issuer = appSettings.JwtIssuer;

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {

                x.IncludeErrorDetails = true;
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            return services;
        }

        private static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {

                var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
                var categories = context.PermissionCategoryPermissions
                    .Include(c => c.Permission)
                    .Include(c => c.Category);


                foreach (var permissionCategory in categories)
                {
                    //Usage : user_add
                    options.AddPolicy(permissionCategory.Category.Label.ToLower() + "_" + permissionCategory.Permission.Label.ToLower(),
                        policy => policy.Requirements.Add(new PermissionRequirement(
                            new PermissionRequirementModel(permissionCategory.PermissionId, permissionCategory.CategoryId)
                        )));

                }

                var permissions = context.Permissions.Where(c => c.IsDirective).ToList();
                foreach (var permission in permissions)
                {
                    //Usage : admin
                    options.AddPolicy(permission.Label.ToLower(),
                        policy => policy.Requirements.Add(new PermissionRequirement(
                            new PermissionRequirementModel(permission.Label)
                        )));
                }

            });
            return services;
        }
    }
}
