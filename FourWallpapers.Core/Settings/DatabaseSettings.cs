using Microsoft.Extensions.Configuration;

namespace FourWallpapers.Core.Settings
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; }
        string DatabaseType { get; }
        string DatabaseName { get; }
    }

    public class DatabaseSettings : IDatabaseSettings
    {
        public DatabaseSettings(string connectionString, string databaseType, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseType = databaseType;
            DatabaseName = databaseName;
        }

        public DatabaseSettings(IConfiguration config)
        {
            ConnectionString = config.GetValue<string>("Database:ConnectionString");
            DatabaseType = config.GetValue<string>("Database:Type").ToLower();
            DatabaseName = config.GetValue<string>("Database:Name");
        }

        public string ConnectionString { get; }
        public string DatabaseType { get; }
        public string DatabaseName { get; }
    }
}