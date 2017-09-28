using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FourWallpapers.Core.Settings;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Models.Requests;
using FourWallpapers.Models.Responses;
using FourWallpapers.Repositories.SqlServer.Helpers;
using NUglify.Helpers;

namespace FourWallpapers.Repositories.SqlServer
{
    public class SearchRepository : BaseSqlServerRepository<Image>, ISearchRepository
    {
        private string _keywordsSql;
        private string _randomSql;
        private string _recentSql;
        private string _searchSql;
        private string _topSql;
        private string _totalsSql;

        public SearchRepository(IDatabaseSettings db) : base(db)
        {
            InitializeQueries();
        }

        public SearchRepository(string connectionString) : base(connectionString)
        {
            InitializeQueries();
        }

        public async Task<List<SearchResult>> SearchAsync(Search request, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var lookupKeywords = new Dictionary<Guid, Image>();
                var results = await dbConnection.QueryAsync<Image, Keyword, Image>(_searchSql,
                    (img, keyword) => SqlServerUtilities.FormatSqlResultsData(img, keyword, lookupKeywords),
                    request
                );
                return results.DistinctBy(i => i.ImageId).Select(image => new SearchResult(image)).ToList();
            }
        }

        public async Task<StatsResponse> GetStatsAsync(CancellationToken cancellationToken)
        {
            var output = new StatsResponse();

            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var results = await dbConnection.QueryAsync<Result>(_totalsSql);


                //Total image
                output.TopStats.AddRange(results);
            }

            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var results = await dbConnection.QueryAsync<Result>(_keywordsSql);


                //Total image
                output.TopKeywords.AddRange(results);
            }
            output.AsOf = DateTime.Now;
            return output;
        }

        public async Task<List<SearchResult>> GetRandomAsync(CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var lookupKeywords = new Dictionary<Guid, Image>();
                var results = await dbConnection.QueryAsync<Image, Keyword, Image>(_randomSql,
                    (img, keyword) => SqlServerUtilities.FormatSqlResultsData(img, keyword, lookupKeywords),
                    new
                    {
                        PerPage = 100
                    }
                );
                return results.DistinctBy(i => i.ImageId).Select(image => new SearchResult(image)).ToList();
            }
        }

        public async Task<List<SearchResult>> TopImageSearchAsync(Top topRequest,
            CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var lookupKeywords = new Dictionary<Guid, Image>();
                var results = await dbConnection.QueryAsync<Image, Keyword, Image>(_topSql,
                    (img, keyword) => SqlServerUtilities.FormatSqlResultsData(img, keyword, lookupKeywords),
                    topRequest
                );
                return results.DistinctBy(i => i.ImageId).Select(image => new SearchResult(image)).ToList();
            }
        }

        public async Task<List<SearchResult>> GetRecentAsync(CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var lookupKeywords = new Dictionary<Guid, Image>();
                var results = await dbConnection.QueryAsync<Image, Keyword, Image>(_recentSql,
                    (img, keyword) => SqlServerUtilities.FormatSqlResultsData(img, keyword, lookupKeywords),
                    new
                    {
                        PerPage = 100
                    }
                );
                return results.DistinctBy(i => i.ImageId).Select(image => new SearchResult(image)).ToList();
            }
        }

        private void InitializeQueries()
        {
            _searchSql = SqlServerUtilities.GetQuery("Search.Search");
            _randomSql = SqlServerUtilities.GetQuery("Search.Random");
            _topSql = SqlServerUtilities.GetQuery("Search.Top");
            _recentSql = SqlServerUtilities.GetQuery("Search.Recent");
            _totalsSql = SqlServerUtilities.GetQuery("Search.Totals");
            _keywordsSql = SqlServerUtilities.GetQuery("Search.Keywords");
        }
    }
}