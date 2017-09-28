using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FourWallpapers.Core;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Core.Settings;
using FourWallpapers.Repositories.SqlServer.Helpers;

namespace FourWallpapers.Repositories.SqlServer
{
    public class ImageScrapeRepository : BaseSqlServerRepository<ImageScrape>, IImageScrapeRepository
    {
        private string _insertImageScrapeQuery;
        private string _searchHashQuery;
        private string _searchQuery;

        public ImageScrapeRepository(IDatabaseSettings db) : base(db)
        {
            InitializeQueries();
        }

        public ImageScrapeRepository(string connectionString) : base(connectionString)
        {
            InitializeQueries();
        }

        public override async Task<Guid> AddAsync(ImageScrape item, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                item.Id = Guid.NewGuid();
                await dbConnection.ExecuteAsync(_insertImageScrapeQuery, item);
                return item.Id;
            }
        }

        public async Task<bool> ExistsAsync(string imageId, Enums.Sources source, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var results =
                    await dbConnection.QueryFirstAsync<decimal>(_searchQuery, new {ImageId = imageId, Source = source});
                return results > 0;
            }
        }

        public async Task<bool> HashExistsAsync(string hash, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var results =
                    await dbConnection.QueryFirstAsync<decimal>(_searchHashQuery, new {Hash = hash});
                return results > 0;
            }
        }

        private void InitializeQueries()
        {
            _insertImageScrapeQuery = SqlServerUtilities.GetQuery("Scrape.Insert");
            _searchQuery = SqlServerUtilities.GetQuery("Scrape.Search");
            _searchHashQuery = SqlServerUtilities.GetQuery("Scrape.SearchHash");
        }
    }
}