using Microsoft.Extensions.Configuration;

namespace FourWallpapers.Core.Settings
{
    public interface ICacheSettings
    {
        bool Database { get; }
    }

    public class CacheSettings : ICacheSettings
    {
        public CacheSettings(IConfiguration config)
        {
            Database = config.GetValue("Caching:Database", false);
        }
        
        public bool Database { get; }
    }
}