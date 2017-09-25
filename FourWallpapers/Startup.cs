using System.Net;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Models.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FourWallpapers {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment()) {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            //global settings
            var settings = new GlobalSettings(Configuration);
            services.AddSingleton<IGlobalSettings>(settings);
            //configure the memory cache
            services.AddMemoryCache();

            //add configuration to the injector so we can access it from controllers
            services.AddSingleton<IConfiguration>(Configuration);

            //add IDatabaseSettings to the injector so we can access it from anywhere that supports dependency injection
            services.AddSingleton(settings.Database);
            
            //add repositories
            services.AddSingleton<IImageRepository, Repositories.SqlServer.ImageRepository>();

            services.AddSingleton<ISearchRepository, Repositories.SqlServer.SearchRepository>();

            services
                .AddSingleton<IKeywordRepository,
                    Repositories.SqlServer.KeywordRepository>();


            services.ConfigureApplicationCookie(options => options.Events = new CookieAuthenticationEvents {
                OnRedirectToLogin = context => {
                    if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == (int) HttpStatusCode.OK)
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;

                    return Task.FromResult(0);
                }
            });

            //Add framework services.
            services.AddOptions();
            services.AddMvc();
            
            services.AddApplicationInsightsTelemetry(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment()) {
                loggerFactory.AddDebug();
                loggerFactory.AddFile("Logs/FourWallpapers-{Date}.txt", isJson: true);
            }

            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapRoute(
                    "default",
                    "api/{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new {controller = "Home", action = "Index"});
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }
    }
}