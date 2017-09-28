using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Core.Models.Request;
using FourWallpapers.Repositories.SqlServer.Tests.Helpers;
using Xunit;

namespace FourWallpapers.Repositories.SqlServer.Tests
{
    public class ImageRepository : DatabaseUnitTest
    {
        public ImageRepository()
        {
            var keywordRepo = new SqlServer.KeywordRepository(Database.TestConnectionString);
            _imageRepository = new SqlServer.ImageRepository(keywordRepo, Database.TestConnectionString);
        }

        private readonly IImageRepository _imageRepository;

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
            await _imageRepository.AddAsync(newImage, CancellationToken.None);
        }

        [Fact]
        public async Task AddAsync()
        {
            var result = await _imageRepository.AddAsync(newImage, CancellationToken.None);
            //cc1d8acb-07ef-4f6b-b078-4deea8ce83f7
            Assert.NotEqual("00000000-0000-0000-0000-000000000000", result.ToString());
        }

        [Fact]
        public async Task FindAll()
        {
            await LoadData();

            var result = await _imageRepository.FindAllAsync(CancellationToken.None);

            Assert.Single(result);
        }

        [Fact]
        public async Task FindByImageIdAsync()
        {
            await LoadData();
            var result = await _imageRepository.FindByImageIdAsync("001", CancellationToken.None, false);
            Assert.Equal("001", result.ImageId);
        }

        [Fact]
        public async Task UpdateImageAsync()
        {
            await LoadData();

            var update = new Update {Key = "Class", Value = "1"};
            await _imageRepository.UpdateImageAsync("001", update, CancellationToken.None);


            update = new Update {Key = "Keywords", Value = "testing,update"};
            await _imageRepository.UpdateImageAsync("001", update, CancellationToken.None);

            update = new Update {Key = "Report", Value = "Reported"};
            await _imageRepository.UpdateImageAsync("001", update, CancellationToken.None);
            var result = await _imageRepository.FindByImageIdAsync("001", CancellationToken.None, false);

            Assert.Equal(Enums.Classes.SafeForWork, result.Class);
            Assert.Equal(Enums.Reported.Reported, result.Reported);
            Assert.Equal("testing", result.Keywords.First());
        }
    }
}