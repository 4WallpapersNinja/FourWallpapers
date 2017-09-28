using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Core.Models.Request;
using FourWallpapers.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FourWallpapers.Controllers {
    [Route("api/Top")]
    public class TopController : Controller {
        private readonly ISearchRepository _repo;
        private readonly IGlobalSettings _settings;
        private readonly IMemoryCache _memoryCache;

        public TopController(IGlobalSettings globalSettings, ISearchRepository repo, IMemoryCache memoryCache) {
            _repo = repo;
            _memoryCache = memoryCache;
            _settings = globalSettings;
        }

        // GET api/top
        [HttpGet]
        public async Task<List<SearchResult>> Index() {
            var request = new Top() {By = "Total"};

            if (!_settings.Cache.Database) return await _repo.TopImageSearchAsync(request, CancellationToken.None);

            return await _memoryCache.GetOrCreateAsync($"top:total", async cacheEntry => {
                cacheEntry.SetAbsoluteExpiration(new TimeSpan(0, 60, 0));

                return await _repo.TopImageSearchAsync(request, CancellationToken.None);
            });
        }

        // GET api/top/source/{id}
        [HttpGet]
        [Route("source/{id}")]
        public async Task<List<SearchResult>> Source(Enums.Sources id) {
            var request = new Top() {By = "Source", Source = id};

            if (!_settings.Cache.Database) return await _repo.TopImageSearchAsync(request, CancellationToken.None);

            return await _memoryCache.GetOrCreateAsync($"top:source:{id}", async cacheEntry => {
                cacheEntry.SetAbsoluteExpiration(new TimeSpan(0, 60, 0));

                return await _repo.TopImageSearchAsync(request, CancellationToken.None);
            });
        }


        // GET api/top/classification/{id}
        [HttpGet]
        [Route("classification/{id}")]
        public async Task<List<SearchResult>> Source(Enums.Classes id) {
            var request = new Top() {By = "Classification", Class = id};

            if (!_settings.Cache.Database) return await _repo.TopImageSearchAsync(request, CancellationToken.None);

            return await _memoryCache.GetOrCreateAsync($"top:classification:{id}", async cacheEntry => {
                cacheEntry.SetAbsoluteExpiration(new TimeSpan(0, 60, 0));

                return await _repo.TopImageSearchAsync(request, CancellationToken.None);
            });
        }
    }
}