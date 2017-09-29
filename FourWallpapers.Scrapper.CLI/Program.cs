using System;
using System.IO;
using FourWallpapers.Core;
using Microsoft.Extensions.Configuration;

namespace FourWallpapers.Scrapper.CLI
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.UtcNow} :: Scrapper CLI Started!");

            //Build the configuration file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.Production.json", reloadOnChange: true, optional: true)
                .AddJsonFile($"appsettings.Developement.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();


            //build the actual settings class
            var settings = new GlobalSettings(Configuration);

            var scraper = new Scraper(settings);
            scraper.Run();
            Console.WriteLine($"{DateTime.UtcNow} :: Scrapper CLI Finished!");

            Console.ReadLine();
        }
    }
}