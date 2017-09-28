using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FourWallpapers.Core;
using FourWallpapers.Core.Settings;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Models.Requests;
using FourWallpapers.Repositories.SqlServer.Helpers;
using NUglify.Helpers;

namespace FourWallpapers.Repositories.SqlServer
{
    public class ImageRepository : BaseSqlServerRepository<Image>, IImageRepository
    {
        private readonly IKeywordRepository _keywordRepository;

        private string _findAllQuery;
        private string _findByIdQuery;
        private string _incrementAndFindByIdQuery;
        private string _insertQuery;

        public ImageRepository(IKeywordRepository keywordRepo, IDatabaseSettings db) : base(db)
        {
            _keywordRepository = keywordRepo;
            InitializeQueries();
        }

        public ImageRepository(IKeywordRepository keywordRepo, string connectionString) : base(connectionString)
        {
            _keywordRepository = keywordRepo;
            InitializeQueries();
        }

        public override async Task<IEnumerable<Image>> FindAllAsync(CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var lookupKeywords = new Dictionary<Guid, Image>();
                return await dbConnection.QueryAsync<Image, Keyword, Image>(_findAllQuery,
                    (img, keyword) => SqlServerUtilities.FormatSqlResultsData(img, keyword, lookupKeywords)
                );
            }
        }

        public override async Task<Guid> AddAsync(Image item, CancellationToken cancellationToken)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                item.Id = Guid.NewGuid();
                await dbConnection.ExecuteAsync(_insertQuery, item);

                // get the new image
                var me = await FindByIdAsync(item.Id, cancellationToken);

                // loop keywords and find out what ones we need to make
                foreach (var keyword in item.Keywords)
                    await _keywordRepository.LinkImageToKeywordAsync(me.IdKey, keyword, cancellationToken);
                return item.Id;
            }
        }

        public async Task<Image> FindByImageIdAsync(string imageId, CancellationToken cancellationToken,
            bool increment = true)
        {
            using (var dbConnection = Connection())
            {
                dbConnection.Open();
                var lookupKeywords = new Dictionary<Guid, Image>();
                var results = await dbConnection.QueryAsync<Image, Keyword, Image>(
                    increment ? _incrementAndFindByIdQuery : _findByIdQuery,
                    (img, keyword) => SqlServerUtilities.FormatSqlResultsData(img, keyword, lookupKeywords),
                    new
                    {
                        Id = imageId
                    });
                return results.DistinctBy(s => s.Id).FirstOrDefault();
            }
        }

        public async Task UpdateImageAsync(string imageId, Update updateRequest,
            CancellationToken cancellationToken)
        {
            // check if is locked image

            // if not allow an update
            switch (updateRequest.Key)
            {
                case "Class":
                    using (var dbConnection = Connection())
                    {
                        dbConnection.Open();
                        const string updateClassSql = "UPDATE Image SET [Class] = @Class WHERE [ImageId] = @Id";
                        await dbConnection.ExecuteAsync(updateClassSql, new
                        {
                            Id = imageId,
                            Class = updateRequest.Value
                        });
                    }
                    return;
                case "Keywords":
                    var keywords = updateRequest.Value.Split(',');
                    var image = await FindByImageIdAsync(imageId, cancellationToken, false);
                    foreach (var keyword in keywords)
                        await _keywordRepository.LinkImageToKeywordAsync(image.IdKey, keyword, cancellationToken);

                    return;
                case "Report":
                    using (var dbConnection = Connection())
                    {
                        const string updateReportedSql =
                            "UPDATE Image SET [Reported] = @Reported WHERE [ImageId] = @Id";
                        await dbConnection.ExecuteAsync(updateReportedSql, new
                        {
                            Id = imageId,
                            Enums.Reported.Reported
                        });
                    }
                    return;
            }
        }

        private void InitializeQueries()
        {
            _findAllQuery = SqlServerUtilities.GetQuery("Image.FindAll");
            _findByIdQuery = SqlServerUtilities.GetQuery("Image.FindById");
            _incrementAndFindByIdQuery = SqlServerUtilities.GetQuery("Image.Increment") + _findByIdQuery;
            _insertQuery = SqlServerUtilities.GetQuery("Image.Insert");
        }
    }
}