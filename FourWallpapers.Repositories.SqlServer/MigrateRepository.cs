using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FourWallpapers.Core.Settings;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Repositories.SqlServer.Helpers;

namespace FourWallpapers.Repositories.SqlServer
{
    public class MigrateRepository : IMigrateRepository
    {
        public const string SqlInitializationEmbeddedLocation = @"FourWallpapers.Repositories.SqlServer.Initialization";
        public const string SqlMigrationsEmbeddedLocation = @"FourWallpapers.Repositories.SqlServer.Migrations";
        protected readonly string ConnectionString;

        public MigrateRepository(IDatabaseSettings db)
        {
            ConnectionString = db.ConnectionString;
        }

        public MigrateRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();

                // create all tables. all these table files should have existance checks in them
                var initializationSql = SqlServerUtilities
                    .GetEmbeddedFileNames(SqlInitializationEmbeddedLocation)
                    .OrderBy(fileName => int.Parse(fileName.Replace(SqlInitializationEmbeddedLocation + ".", "")
                        .Replace(".sql", "")
                        .Split('-')
                        .First()))
                    .ToList()
                    .Select(SqlServerUtilities.GetEmbeddedFile).SelectMany(
                        filecontents => Regex.Split(filecontents, @"^GO.*$", RegexOptions.Multiline)
                            .Where(s => !string
                                .IsNullOrWhiteSpace(
                                    s)) // we split the file on the GO statements and run each section as its own query
                    ).ToArray();

                foreach (var sql in initializationSql)
                    // execute each update
                    await dbConnection.ExecuteAsync(sql);
            }

            await MigrateAsync(cancellationToken);
        }

        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();

                var migrationSqlFilenames = SqlServerUtilities
                    .GetEmbeddedFileNames(SqlMigrationsEmbeddedLocation)
                    .OrderBy(filename => int.Parse(filename.Replace(SqlMigrationsEmbeddedLocation + ".", "")
                        .Replace(".sql", "").Split('-').First())).ToDictionary(
                        key => int.Parse(key.Replace(SqlMigrationsEmbeddedLocation, "").Replace(".sql", "").Split('-')
                            .First()), value => value);

                var lastMigration = 0;

                // these should be run in numerical order
                foreach (var sqlFiles in migrationSqlFilenames)
                {
                    if (await CheckAsync(cancellationToken) <= sqlFiles.Key) continue;
                    var fileContents = SqlServerUtilities.GetEmbeddedFile(sqlFiles.Value);

                    // we split the file on the GO statements and run each section as its own query
                    var sqlQueries = Regex.Split(fileContents, @"^GO.*$", RegexOptions.Multiline)
                        .Where(s => !string.IsNullOrWhiteSpace(s));

                    foreach (var sql in sqlQueries)
                        // execute each update
                        await dbConnection.ExecuteAsync(sql);

                    lastMigration = sqlFiles.Key;
                }

                // update to current migration
                await dbConnection.ExecuteAsync(
                    "UPDATE [Configuration] SET [Value] = @lastMigration WHERE [Id] = 'MigrationVersion'",
                    new {lastMigration});
            }
        }

        public async Task<int> CheckAsync(CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();

                return await dbConnection.QueryFirstAsync<int>(
                    "SELECT [Value] FROM [Configuration] WHERE [Id] = 'MigrationVersion'");
            }
        }

        public async Task CleanAsync(CancellationToken cancellationToken)
        {
            //drops all data from every table
            var query = "EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'; " +
                        "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'; " +
                        "EXEC sp_MSForEachTable 'DELETE FROM ?'; " +
                        "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'; " +
                        "EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?';";

            using (var dbConnection = Connection())
            {
                dbConnection.Open();

                await dbConnection.ExecuteAsync(query);
            }
        }

        protected IDbConnection Connection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}