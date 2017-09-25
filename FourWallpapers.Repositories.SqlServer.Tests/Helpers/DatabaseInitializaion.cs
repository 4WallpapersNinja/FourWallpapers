using System;
using Xunit;

namespace FourWallpapers.Repositories.SqlServer.Tests.Helpers
{
    internal class DatabaseInitializationFixture : IDisposable
    {
        public string DatabaseName;
        public string DatabasePath;

        public DatabaseInitializationFixture()
        {
            DatabaseName = Database.DatabaseName;
            DatabasePath = Database.DatabasePath;

            Database.Initialize(DatabaseName, DatabasePath);
        }

        public void Dispose()
        {
            try
            {
                Database.DetachDatabase(DatabaseName);
            }
            catch
            {
                //ignore errors when detaching
            }
        }
    }

    /// <summary>
    ///     Empty Collection class to hold the DB Collection Definition to init the DatabaseInitFixture
    /// </summary>
    [CollectionDefinition("DB")]
    public class DatabaseInitCollection : ICollectionFixture<DatabaseInitializationFixture>
    {
    }

    /// <summary>
    ///     Helper parent class to add Database accessing functionality to test classes
    /// </summary>
    [Collection("DB")]
    [AutoRollback]
    public class DatabaseUnitTest
    {
    }
}