using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FourWallpapers.Core;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Repositories.SqlServer;

namespace FourWallpapers.Scrapper
{
    public class ScrapeRepositories
    {
        public readonly HttpClient Client;

        public readonly IGlobalSettings GlobalSettings;
        public readonly IImageRepository ImageRepository;
        public readonly IImageScrapeRepository ImageScrapeRepository;

        public readonly Queue<ImageDetail> Queue;

        public ScrapeRepositories(IGlobalSettings globalSettings)
        {
            GlobalSettings = globalSettings;

            //setup the download queue 
            Queue = new Queue<ImageDetail>();


            switch (GlobalSettings.Database.DatabaseType.ToLower())
            {
                case "sqlserver":
                    //add repositories
                    var keywordRepo = new KeywordRepository(globalSettings.Database);
                    ImageRepository = new ImageRepository(keywordRepo, globalSettings.Database);
                    ImageScrapeRepository = new ImageScrapeRepository(globalSettings.Database);
                    break;
            }


            //setup httpClient
            ServicePointManager.DefaultConnectionLimit = 10000;
            ServicePointManager.ReusePort = true;
            var cookieContainer = new CookieContainer();

            cookieContainer.Add(new Uri(@"https://www.reddit.com"), new Cookie("over18", "1"));

            //set the httpclient to auto-decompress gzup and deflate encodings
            var httpClientHandler = new ScraperHttpHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                CookieContainer = cookieContainer,
                AllowAutoRedirect = true
            };

            Client = new HttpClient(httpClientHandler);
            Client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.7) Gecko/20091221 Firefox/3.5.7 (.NET CLR 3.5.30729)");
        }
    }
}