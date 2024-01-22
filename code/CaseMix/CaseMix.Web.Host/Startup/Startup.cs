using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Json;
using Amazon.S3;
using CaseMix.Attributes;
using CaseMix.Aws.Email;
using CaseMix.Aws.S3.Model;
using CaseMix.Configuration;
using CaseMix.Identity;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using SnomedApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;

namespace CaseMix.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private const string _apiVersion = "v1";
        private const string _thirdPartyApiVersion = "tpv1";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.Configure<EmailConfiguration>(_appConfiguration.GetSection("Email"));
            services.Configure<SnomedApiConfiguration>(_appConfiguration.GetSection("SnomedApi"));
            services.Configure<MemcachedConfiguration>(_appConfiguration.GetSection("Memcached"));
            services.Configure<AwsConfiguration>(_appConfiguration.GetSection("Aws"));

            //MVC
            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                }
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            //services.AddSignalR();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_apiVersion, new OpenApiInfo
                {
                    Version = _apiVersion,
                    Title = "CaseMix API",
                    Description = "CaseMix",
                });
                options.SwaggerDoc(_thirdPartyApiVersion, new OpenApiInfo
                {
                    Version = _thirdPartyApiVersion,
                    Title = "CaseMix API for Third Party Access",
                    Description = "CaseMix Third Patty",
                });

                options.DocInclusionPredicate((version, desc) =>
                {
                    if (version == _thirdPartyApiVersion)
                    {
                        desc.TryGetMethodInfo(out MethodInfo methodInfo);
                        if (methodInfo == null)
                        {
                            return false;
                        }
                        var thirdPartyApi = methodInfo.DeclaringType.GetCustomAttribute<ThirdPartyApiAttribute>();
                        if (thirdPartyApi == null)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        desc.TryGetMethodInfo(out MethodInfo methodInfo);
                        if (methodInfo == null)
                        {
                            return false;
                        }
                        var thirdPattyApi = methodInfo.DeclaringType.GetCustomAttribute<ThirdPartyApiAttribute>();
                        if (thirdPattyApi != null)
                        {
                            return false;
                        }
                    }
                    return true;
                });

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            services.AddEnyimMemcached(option => option.AddServer(_appConfiguration.GetValue<string>("Memcached:Address"), _appConfiguration.GetValue<int>("Memcached:Port")));

            // Configure Abp and Dependency Injection
            services.AddAbpWithoutCreatingServiceProvider<CaseMixWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );

            services.AddAWSService<IAmazonS3>();
        }


        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });

            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();

            app.UseEnyimMemcached();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                // specifying the Swagger JSON endpoint.
                options.SwaggerEndpoint(_appConfiguration["App:ServerBasePath"] + $"/swagger/{_apiVersion}/swagger.json", $"CaseMix API {_apiVersion}");
                options.SwaggerEndpoint(_appConfiguration["App:ServerBasePath"] + $"/swagger/{_thirdPartyApiVersion}/swagger.json", $"CaseMix API {_thirdPartyApiVersion}");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("CaseMix.Web.Host.wwwroot.swagger.ui.index.html");
                options.DisplayRequestDuration(); // Controls the display of the request duration (in milliseconds) for "Try it out" requests.
                options.ConfigObject.AdditionalItems.Add("environment", _appConfiguration["App:ServerBasePath"]);
            }); // URL: /swagger
        }
    }
}
