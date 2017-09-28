using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using FourWallpapers.Core.Database.Repositories;

namespace FourWallpapers.Repositories.SqlServer.Tests.Helpers
{
    public static class Database
    {
        public const string DatabaseName = "FourWallpapersNinjaTestDb";
        public const string DatabasePath = @"::DataDirectory::\Data\FourWallpapersNinjaTestDb.mdf";

        public const string ConnectionString =
                @"Server=(LocalDB)\MSSQLLocalDB;AttachDbFileName=::DatabasePath::;Initial Catalog=::DatabaseName::;Integrated Security=True;MultipleActiveResultSets=True"
            ;

        public static IMigrateRepository MigrateRepo;

        public static string TestConnectionString = "";

        public static void Initialize(string databaseName, string databasePath)
        {
            var filePath = databasePath.Replace("::DataDirectory::", AppDomain.CurrentDomain.BaseDirectory);
            // need to assemble paths for localDB
            var databaseFileName = Path.Combine(filePath, $"{databaseName}.mdf");
            var databaseLogFileName = Path.Combine(filePath, $"{databaseName}_log.ldf");

            // create the path if it doesn't exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            try
            {
                // database exists delete it
                if (File.Exists(databaseFileName))
                    File.Delete(databaseFileName);

                // log file exists delete it
                if (File.Exists(databaseLogFileName))
                    File.Delete(databaseLogFileName);
            }
            catch
            {
                //ignore any issues caused by the deletes
            }
            // create database 
            CreateDatabase(databaseName, databaseFileName);

            // setup repository
            MigrateRepo = new MigrateRepository(GetConnectionString(databaseName, databaseFileName));

            PopulateDatabase();
        }

        /// <summary>
        ///     Creates the database file at databaseFilePath
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="databaseFilePath"></param>
        public static void CreateDatabase(string databaseName, string databaseFilePath)
        {
            var connectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                // detach if the db exists
                try
                {
                    DetachDatabase(databaseName);
                }
                catch
                {
                    //ignore errors when detaching
                }

                //create the db
                cmd.CommandText =
                    $"CREATE DATABASE {databaseName} ON (NAME = N'{databaseName}', FILENAME = '{databaseFilePath}')";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Gets the connection string to the newly created database
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="databaseFilePath"></param>
        /// <returns></returns>
        private static string GetConnectionString(string databaseName, string databaseFilePath)
        {
            if (TestConnectionString == "")
                TestConnectionString = ConnectionString.Replace("::DatabaseName::", databaseName)
                    .Replace("::DatabasePath::", databaseFilePath);
            return TestConnectionString;
        }

        /// <summary>
        ///     Populates the database based on all .sql files at SqlFileLocation
        /// </summary>
        public static void PopulateDatabase()
        {
            MigrateRepo.InitializeAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Cleans the database by deleting all rows from every table
        /// </summary>
        public static void CleanDatabase()
        {
            MigrateRepo.CleanAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        //detaches the db from the localdb instance so it is deletable later
        public static void DetachDatabase(string dbName)
        {
            var connectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = $"exec sp_detach_db '{dbName}'";
                cmd.ExecuteNonQuery();
            }
        }
    }
}