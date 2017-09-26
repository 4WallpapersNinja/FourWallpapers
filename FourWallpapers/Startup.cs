using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;
using Identity.Dapper;
using Identity.Dapper.Entities;
using Identity.Dapper.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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

            services.ConfigureDapperSqlServerConnectionProvider(Configuration.GetSection("DapperIdentity"))
                .ConfigureDapperIdentityCryptography(Configuration.GetSection("DapperIdentityCryptography"));

            services.AddIdentity<DapperIdentityUser, DapperIdentityRole>()
                .AddDapperIdentityForSqlServer()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                });

            services.AddApplicationInsightsTelemetry(Configuration);

            //Add framework services.
            services.AddOptions();
            services.AddMvc();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment()) {
                loggerFactory.AddDebug();
                loggerFactory.AddFile("Logs/FourWallpapers-{Date}.txt", isJson: true);
            }

            app.UseStaticFiles();

            app.UseAuthentication();

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