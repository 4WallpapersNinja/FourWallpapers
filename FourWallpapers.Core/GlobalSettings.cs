using FourWallpapers.Core.Settings;
using Microsoft.Extensions.Configuration;

namespace FourWallpapers.Core
{
    public interface IGlobalSettings
    {
        ICacheSettings Cache { get; }
        IDatabaseSettings Database { get; }
        IRedisSettings Redis { get; }
        IScrapeSettings Scraper { get; }

        /// <summary>
        ///     Whether or not request logging is enabled
        /// </summary>
        bool LogRequests { get; }
    }

    public class GlobalSettings : IGlobalSettings
    {
        public GlobalSettings(IConfiguration config)
        {
            Cache = new CacheSettings(config);
            Database = new DatabaseSettings(config);
            Redis = new RedisSettings(config);
            Scraper = new ScrapeSettings(config);

            LogRequests = config.GetValue("LogRequests", false);
        }

        public ICacheSettings Cache { get; }
        public IDatabaseSettings Database { get; }
        public IRedisSettings Redis { get; }
        public IScrapeSettings Scraper { get; }

        public bool LogRequests { get; }
    }
}