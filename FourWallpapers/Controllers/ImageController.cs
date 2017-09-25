using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FourWallpapers.Controllers {
    [Route("api/Image")]
    public class ImageController : Controller {
        private readonly IImageRepository _repo;

        public ImageController(IImageRepository repo) {
            _repo = repo;
        }

        // GET api/Image/{imageId}
        [HttpGet]
        [Route("{imageId}")]
        public async Task<Image> GetImage(string imageId) {
            return await _repo.FindByImageIdAsync(imageId, CancellationToken.None);
        }

        // GET api/Image/{imageId}/update
        [HttpPost]
        [Route("{imageId}/update")]
        public async Task Keywords(string imageId, [FromBody] UpdateRequest updateRequest) {
            await _repo.UpdateImageAsync(imageId, updateRequest, CancellationToken.None);
        }
    }
}