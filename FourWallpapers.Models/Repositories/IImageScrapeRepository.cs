using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;

namespace FourWallpapers.Models.Repositories
{
    public interface IImageScrapeRepository : IRepository<ImageScrape>
    {
        Task<bool> ExistsAsync(string imageId, Enums.Sources source, CancellationToken cancellationToken);
        Task<bool> HashExistsAsync(string hash, CancellationToken cancellationToken);
    }
}