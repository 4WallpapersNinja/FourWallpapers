using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Models.Requests;
using FourWallpapers.Repositories.SqlServer.Tests.Helpers;
using Xunit;

namespace FourWallpapers.Repositories.SqlServer.Tests
{
    public class SearchRepository : DatabaseUnitTest
    {
        public SearchRepository()
        {
            _searchRepository = new SqlServer.SearchRepository(Database.TestConnectionString);
            IKeywordRepository keywordRepository = new SqlServer.KeywordRepository(Database.TestConnectionString);
            _imageRepository = new SqlServer.ImageRepository(keywordRepository, Database.TestConnectionString);
        }

        private readonly ISearchRepository _searchRepository;
        private readonly IImageRepository _imageRepository;

        private async Task LoadData()
        {
            var listOfImages = new List<Image>()
            {
                new Image
                {
                    ImageId = "001",
                    Class = Enums.Classes.Unrated,
                    IndexSource = Enums.Sources.RedditEarthPorn,
                    Who = "Me",
                    Tripcode = "",
                    ResolutionX = 1024,
                    ResolutionY = 768,
                    TagsString = "None",
                    Keywords = new List<string>() {"rule34"},
                    DateDownloaded = DateTime.UtcNow,
                    Reported = Enums.Reported.Unreported,
                    Hash = "0x0",
                    FileExtension = "png",
                    ServerId = 0,
                    Size = 0,
                    Ratio = "4:3"
                },
                new Image
                {
                    ImageId = "002",
                    Class = Enums.Classes.SafeForWork,
                    IndexSource = Enums.Sources.RedditHiRes,
                    Who = "Me",
                    Tripcode = "",
                    ResolutionX = 2560,
                    ResolutionY = 1600,
                    TagsString = "None",
                    Keywords = new List<string>() {"test"},
                    DateDownloaded = DateTime.UtcNow,
                    Reported = Enums.Reported.ConfirmedGood,
                    Hash = "0x0",
                    FileExtension = "png",
                    ServerId = 0,
                    Size = 0,
                    Ratio = "16:9"
                },
                new Image
                {
                    ImageId = "003",
                    Class = Enums.Classes.SafeForWork,
                    IndexSource = Enums.Sources.RedditSpacePorn,
                    Who = "Me",
                    Tripcode = "",
                    ResolutionX = 3840,
                    ResolutionY = 2160,
                    TagsString = "None",
                    Keywords = new List<string>() {"rule34", "test"},
                    DateDownloaded = DateTime.UtcNow,
                    Reported = Enums.Reported.Unreported,
                    Hash = "0x0",
                    FileExtension = "png",
                    ServerId = 0,
                    Size = 0,
                    Ratio = "16:9"
                },
                new Image
                {
                    ImageId = "004",
                    Class = Enums.Classes.NotSafeForWork,
                    IndexSource = Enums.Sources.RedditNsfwWallpapers,
                    Who = "Me",
                    Tripcode = "",
                    ResolutionX = 1280,
                    ResolutionY = 720,
                    TagsString = "None",
                    Keywords = new List<string>() {"rule34", "test", "foobar"},
                    DateDownloaded = DateTime.UtcNow,
                    Reported = Enums.Reported.Reported,
                    Hash = "0x0",
                    FileExtension = "png",
                    ServerId = 0,
                    Size = 0,
                    Ratio = "16:9"
                },
            };

            foreach (var newImage in listOfImages)
            {
                await _imageRepository.AddAsync(newImage, CancellationToken.None);
            }
        }

        [Fact]
        public async Task SearchAsyncKeywordAndClassification()
        {
            await LoadData();

            //keyword + 
            var request = new Search()
            {
                Class = Enums.Classes.SafeForWork,
                Criteria = "rule34",
                Page = 1,
                Ratio = "Any",
                Resolution = "",
                ResolutionSearch = Enums.ResolutionSearch.Any,
                PerPage = 100,
                Size = 0,
                Source = Enums.Sources.Any
            };

            var results = await _searchRepository.SearchAsync(request, CancellationToken.None);

            Assert.Single(results);
        }

        [Fact]
        public async Task SearchAsyncAll()
        {
            await LoadData();

            //keyword + 
            var request = new Search()
            {
                Class = Enums.Classes.Any,
                Criteria = "",
                Page = 1,
                Ratio = "Any",
                Resolution = "",
                ResolutionSearch = Enums.ResolutionSearch.Any,
                PerPage = 100,
                Size = 0,
                Source = Enums.Sources.Any
            };
            var results = await _searchRepository.SearchAsync(request, CancellationToken.None);

            Assert.Equal(3, results.Count);
        }

        [Fact]
        public async Task TopImageSearchAsyncSource()
        {
            await LoadData();
            var request = new Top()
            {
                By = "source",
                Source = Enums.Sources.RedditEarthPorn
            };

            var topResults = await _searchRepository.TopImageSearchAsync(request, CancellationToken.None);
            Assert.Single(topResults);
        }

        [Fact]
        public async Task TopImageSearchAsyncClassification()
        {
            await LoadData();
            var request = new Top()
            {
                By = "classification",
                Class = Enums.Classes.SafeForWork
            };

            var topResults = await _searchRepository.TopImageSearchAsync(request, CancellationToken.None);
            Assert.Equal(2, topResults.Count);
        }

        [Fact]
        public async Task TopImageSearchAsyncTotal()
        {
            await LoadData();
            var request = new Top()
            {
                By = "total"
            };

            var topResults = await _searchRepository.TopImageSearchAsync(request, CancellationToken.None);
            Assert.Equal(3, topResults.Count);
        }

        [Fact]
        public async Task GetRecentAsync()
        {
            await LoadData();
            var results = await _searchRepository.GetRecentAsync(CancellationToken.None);
            Assert.Equal(3, results.Count);
        }

        [Fact]
        public async Task GetStatsAsync()
        {
            await LoadData();
            var results = await _searchRepository.GetStatsAsync(CancellationToken.None);

            Assert.Equal(3, results.TopStats[0].Count);
            Assert.Equal(2, results.TopKeywords.Count);
        }

        [Fact]
        public async Task GetRandomAsync()
        {
            await LoadData();
            var results = await _searchRepository.GetRandomAsync(CancellationToken.None);
            Assert.Equal(3, results.Count);
        }
    }
}