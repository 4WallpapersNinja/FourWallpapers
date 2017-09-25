using Microsoft.Extensions.Configuration;

namespace FourWallpapers.Core.Settings
{
    public interface ICacheSettings
    {
        bool Page { get; }
        bool Database { get; }

        bool UseRedis { get; }
    }

    public class CacheSettings : ICacheSettings
    {
        public CacheSettings(IConfiguration config)
        {
            Page = config.GetValue("Caching:ConnectionString", false);
            Database = config.GetValue("Caching:Database", false);
            UseRedis = config.GetValue("Caching:UseRedis", false);
        }

        public bool Page { get; }
        public bool Database { get; }
        public bool UseRedis { get; }
    }
}