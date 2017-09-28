using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FourWallpapers.Controllers {
    [Route("api/Recent")]
    public class RecentController : Controller {
        private readonly ISearchRepository _repo;
        private readonly IGlobalSettings _settings;
        private readonly IMemoryCache _memoryCache;

        public RecentController(IGlobalSettings globalSettings, ISearchRepository repo, IMemoryCache memoryCache) {
            _repo = repo;
            _memoryCache = memoryCache;
            _settings = globalSettings;
        }


        // GET api/Recent
        [HttpGet]
        public async Task<List<SearchResult>> Index() {
            if (!_settings.Cache.Database) return await _repo.GetRecentAsync(CancellationToken.None);
            return await _memoryCache.GetOrCreateAsync($"recent", async cacheEntry => {
                cacheEntry.SetAbsoluteExpiration(new TimeSpan(0, 30, 0));

                return await _repo.GetRecentAsync(CancellationToken.None);
            });
        }
    }
}