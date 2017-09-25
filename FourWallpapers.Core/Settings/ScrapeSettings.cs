using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace FourWallpapers.Core.Settings
{
    public interface IScrapeSettings
    {
        /// <summary>
        ///     Max Number of Pages Per Board to Crawl
        /// </summary>
        int MaxPages { get; }

        List<Enums.Sources> Boards { get; }

        string ThumbnailLocation { get; set; }
        string ImageLocation { get; set; }
        int ScrapeServer { get; set; }
    }

    public class ScrapeSettings : IScrapeSettings
    {
        public ScrapeSettings(IConfiguration config)
        {
            MaxPages = config.GetValue("Scraper:MaxPages", 2);
            ThumbnailLocation = config.GetValue("Scraper:ThumbnailLocation", "");
            ImageLocation = config.GetValue("Scraper:ImageLocation", "");
            ScrapeServer = config.GetValue("Scraper:ScrapeServer", 2);

            var boardsString = config.GetValue("Scraper:ActiveBoards", "");
            Boards = string.IsNullOrWhiteSpace(boardsString)
                ? Constants.SourceUrls.Select(kvp => kvp.Key).ToList()
                : boardsString.Split(',').Select(value => (Enums.Sources) int.Parse(value)).ToList();
        }

        public ScrapeSettings(int maxPages, List<Enums.Sources> boards)
        {
            MaxPages = maxPages;
            Boards = boards;
        }

        public int MaxPages { get; }
        public List<Enums.Sources> Boards { get; }
        public string ThumbnailLocation { get; set; }
        public string ImageLocation { get; set; }
        public int ScrapeServer { get; set; }
    }
}