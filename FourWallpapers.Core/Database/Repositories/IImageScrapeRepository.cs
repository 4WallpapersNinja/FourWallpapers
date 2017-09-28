using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core.Database.Entities;

namespace FourWallpapers.Core.Database.Repositories
{
    public interface IImageScrapeRepository : IRepository<ImageScrape>
    {
        Task<bool> ExistsAsync(string imageId, Enums.Sources source, CancellationToken cancellationToken);
        Task<bool> HashExistsAsync(string hash, CancellationToken cancellationToken);
    }
}