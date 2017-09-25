using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Repositories.SqlServer.Tests.Helpers;
using Xunit;

namespace FourWallpapers.Repositories.SqlServer.Tests
{
    public class KeywordRepository : DatabaseUnitTest
    {
        public KeywordRepository()
        {
            _keywordRepository = new SqlServer.KeywordRepository(Database.TestConnectionString);
            _imageRepository = new SqlServer.ImageRepository(_keywordRepository, Database.TestConnectionString);
        }

        private readonly IKeywordRepository _keywordRepository;
        private readonly IImageRepository _imageRepository;

        private Keyword newKeyword => new Keyword
        {
            Value = "test"
        };

        private Image newImage => new Image
        {
            ImageId = "001",
            Class = Enums.Classes.Any,
            IndexSource = Enums.Sources.Any,
            Who = "Me",
            Tripcode = "",
            ResolutionX = 1920,
            ResolutionY = 1080,
            TagsString = "None",
            Keywords = new List<string>() {"rule34"},
            DateDownloaded = DateTime.UtcNow,
            Reported = Enums.Reported.Unreported,
            Hash = "0x0",
            FileExtension = "png",
            ServerId = 0,
            Size = 0,
            Ratio = "16:9"
        };

        private async Task LoadData()
        {
            await _keywordRepository.AddAsync(new Keyword
            {
                Value = "test"
            }, CancellationToken.None);

            await _keywordRepository.AddAsync(new Keyword
            {
                Value = "ipsum"
            }, CancellationToken.None);

            await _keywordRepository.AddAsync(new Keyword
            {
                Value = "bar"
            }, CancellationToken.None);

            await _keywordRepository.AddAsync(new Keyword
            {
                Value = "foo"
            }, CancellationToken.None);

            await _imageRepository.AddAsync(newImage, CancellationToken.None);
        }

        [Fact]
        public async Task AddAsync()
        {
            var result = await _keywordRepository.AddAsync(newKeyword, CancellationToken.None);
            Assert.NotEqual("00000000-0000-0000-0000-000000000000", result.ToString());
        }

        [Theory]
        [InlineData("ipsum")]
        [InlineData("nonexistantkeyword")]
        public async Task LinkImageToKeywordAsync(string keyword)
        {
            await LoadData();
            
            // get image key
            var imageKey = await _imageRepository.FindByImageIdAsync("001", CancellationToken.None);

            //run the test
            await _keywordRepository.LinkImageToKeywordAsync(imageKey.IdKey, keyword, CancellationToken.None);

            imageKey = await _imageRepository.FindByImageIdAsync("001", CancellationToken.None);

            Assert.Contains(imageKey.Keywords, k => k.ToLower() == keyword);
        }

        [Theory]
        [InlineData("ipsum" , false)]
        [InlineData("nonexistantkeyword",true)]
        public async Task FindKeywordKeyAsync(string keyword, bool shouldBeNull)
        {
            await LoadData();

            var result = await _keywordRepository.FindKeywordKeyAsync(keyword, CancellationToken.None);
            if (shouldBeNull)
            {
                Assert.Equal(0,result);
            }
            else
            {
                Assert.True(result >= 1);
            }
        }

        [Theory]
        [InlineData("rule34", true)]
        [InlineData("nonexistantkeyword", false)]
        public async Task DoesLinkExistAsync(string keyword, bool shouldBeTrue)
        {
            await LoadData();
            // get image key
            var imageKey = await _imageRepository.FindByImageIdAsync("001", CancellationToken.None);
            var keywordKey = await _keywordRepository.FindKeywordKeyAsync(keyword, CancellationToken.None);

            var result = await _keywordRepository.DoesLinkExistAsync(imageKey.IdKey, keywordKey ?? 0, CancellationToken.None);

            if (shouldBeTrue)
            {
                Assert.True(result);
            }
            else
            {
                Assert.False(result);
            }
        }
    }
}