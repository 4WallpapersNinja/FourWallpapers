using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Models.Request;

namespace FourWallpapers.Core.Database.Repositories
{
    public interface IImageRepository : IRepository<Image>
    {
        /// <summary>
        ///     Find an image
        /// </summary>
        /// <param name="imageId">imageId of the image your looking for</param>
        /// <param name="cancellationToken">task cancellation token</param>
        /// <param name="increment">if you would like to increment the image download counter</param>
        /// <returns></returns>
        Task<Image> FindByImageIdAsync(string imageId, CancellationToken cancellationToken, bool increment = true);

        /// <summary>
        ///     Update image with the request
        /// </summary>
        /// <param name="imageId">imageId to update</param>
        /// <param name="updateRequest">updates requested</param>
        /// <param name="cancellationToken">task cancellation token</param>
        /// <returns></returns>
        Task UpdateImageAsync(string imageId, Update updateRequest, CancellationToken cancellationToken);
    }
}