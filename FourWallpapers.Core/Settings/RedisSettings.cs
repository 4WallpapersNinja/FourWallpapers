using Microsoft.Extensions.Configuration;

namespace FourWallpapers.Core.Settings
{
    public interface IRedisSettings
    {
        string ConnectionString { get; }
        int Database { get; }
        string Server { get; }
        int Port { get; }
    }

    public class RedisSettings : IRedisSettings
    {
        public RedisSettings(IConfiguration config)
        {
            ConnectionString = config.GetValue("Redis:ConnectionString", "");
            Database = config.GetValue("Redis:Database", 0);
            Server = config.GetValue("Redis:Server", "localhost");
            Port = config.GetValue("Redis:Port", 6379);

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                //assemble the connection string dynamicly
            }
        }

        public string ConnectionString { get; }
        public int Database { get; }
        public string Server { get; }
        public int Port { get; }
    }
}