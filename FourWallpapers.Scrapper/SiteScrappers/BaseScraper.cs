using FourWallpapers.Core.Settings;

namespace FourWallpapers.Scrapper.SiteScrappers
{
    public class BaseScraper
    {
        public readonly ScrapeRepositories ScrapeRepositories;

        public BaseScraper(ScrapeRepositories scrapeRepositories)
        {
            ScrapeRepositories = scrapeRepositories;
        }

        public IScrapeSettings Settings => ScrapeRepositories.GlobalSettings.Scraper;
    }
}