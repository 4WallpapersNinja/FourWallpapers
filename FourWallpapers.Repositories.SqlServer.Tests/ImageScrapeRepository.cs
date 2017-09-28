using System;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Repositories.SqlServer.Tests.Helpers;
using Xunit;

namespace FourWallpapers.Repositories.SqlServer.Tests
{
    public class ImageScrapeRepository : DatabaseUnitTest
    {
        public ImageScrapeRepository()
        {
            _imageScrapeRepository = new SqlServer.ImageScrapeRepository(Database.TestConnectionString);
        }

        private readonly IImageScrapeRepository _imageScrapeRepository;


        private ImageScrape newImageScrape => new ImageScrape
        {
            ImageId = "001",
            Source = Enums.Sources.Any,
            Hash = "0x0"
        };

        private async Task LoadData()
        {
            await _imageScrapeRepository.AddAsync(newImageScrape, CancellationToken.None);
        }

        [Fact]
        public async Task AddAsync()
        {
            var result = await _imageScrapeRepository.AddAsync(newImageScrape, CancellationToken.None);
            Assert.NotEqual("00000000-0000-0000-0000-000000000000", result.ToString());
        }

        [Fact]
        public async Task ExistsAsync()
        {
            await LoadData();

            var result = await _imageScrapeRepository.ExistsAsync("001", Enums.Sources.Any, CancellationToken.None);
            Assert.True(result);

            result = await _imageScrapeRepository.ExistsAsync("NonExistantId", Enums.Sources.Any,
                CancellationToken.None);
            Assert.False(result);
        }

        [Fact]
        public async Task HashExistsAsync()
        {
            await LoadData();

            var result = await _imageScrapeRepository.HashExistsAsync("0x0", CancellationToken.None);
            Assert.True(result);

            result = await _imageScrapeRepository.HashExistsAsync("NonExistantHash", CancellationToken.None);
            Assert.False(result);
        }
    }
}