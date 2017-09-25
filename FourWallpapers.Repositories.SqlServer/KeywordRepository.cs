using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FourWallpapers.Core.Settings;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Repositories.SqlServer.Helpers;

namespace FourWallpapers.Repositories.SqlServer
{
    public class KeywordRepository : BaseSqlServerRepository<Keyword>, IKeywordRepository
    {
        private string _checkLinkQuery;
        private string _findKeywordQuery;
        private string _insertKeywordQuery;
        private string _insertLinkQuery;

        public KeywordRepository(IDatabaseSettings db) : base(db)
        {
            InitializeQueries();
        }

        public KeywordRepository(string connectionString) : base(connectionString)
        {
            InitializeQueries();
        }

        public override async Task<Guid> AddAsync(Keyword item, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                item.Id = Guid.NewGuid();

                await dbConnection.ExecuteAsync(_insertKeywordQuery, item);

                return item.Id;
            }
        }

        public async Task LinkImageToKeywordAsync(decimal imageKey, string keyword,
            CancellationToken cancellationToken)
        {
            //get keyword
            if (string.IsNullOrWhiteSpace(keyword)) return;

            var keywordKey = await FindKeywordKeyAsync(keyword, cancellationToken) ?? -1m;
            if (keywordKey < 1)
            {
                //create new keyword
                var newKeyword = new Keyword {Value = keyword};
                await AddAsync(newKeyword, cancellationToken);

                keywordKey = await FindKeywordKeyAsync(keyword, cancellationToken) ?? -1m;
                if (keywordKey < 1) throw new Exception("Unable to find keyword after insert");
            }

            //does the link already exist?
            if (await DoesLinkExistAsync(imageKey, keywordKey, cancellationToken)) return;

            using (var dbConnection = Connection())
            {
                dbConnection.Open();

                try
                {
                    //link doesnt exist insert one
                    await dbConnection.ExecuteAsync(_insertLinkQuery, new
                    {
                        ImageIdKey = imageKey,
                        KeywordIdKey = keywordKey
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<decimal?> FindKeywordKeyAsync(string keyword, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                try
                {
                    return await dbConnection.QueryFirstOrDefaultAsync<decimal>(_findKeywordQuery,
                        new
                        {
                            Keyword = keyword.ToLower()
                        });
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<bool> DoesLinkExistAsync(decimal imageKey, decimal keywordKey, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();

                var results = await dbConnection.QueryFirstOrDefaultAsync<decimal>(_checkLinkQuery,
                    new
                    {
                        KeywordIdKey = keywordKey,
                        ImageIdKey = imageKey
                    });
                return results == 1m;
            }
        }

        private void InitializeQueries()
        {
            _findKeywordQuery = SqlServerUtilities.GetQuery("Keyword.Find");
            _insertLinkQuery = SqlServerUtilities.GetQuery("Keyword.InsertLink");
            _insertKeywordQuery = SqlServerUtilities.GetQuery("Keyword.Insert");
            _checkLinkQuery = SqlServerUtilities.GetQuery("Keyword.CheckLink");
        }

    }
}