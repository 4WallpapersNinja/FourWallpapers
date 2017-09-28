using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FourWallpapers.Controllers {
    [Route("api/General")]
    public class GeneralController : Controller {
        private readonly ISearchRepository _repo;
        private readonly IMemoryCache _memoryCache;
        private readonly IGlobalSettings _settings;

        public GeneralController(IGlobalSettings globalSettings, ISearchRepository repo, IMemoryCache memoryCache) {
            _repo = repo;
            _memoryCache = memoryCache;
            _settings = globalSettings;
        }

        // GET api/General/Test
        [HttpGet]
        [Route("Test")]
        public async Task<SearchResult> Test() {
            var results = await _repo.GetRecentAsync(CancellationToken.None);
            return results.First();
        }

        [HttpGet]
        [Route("stats")]
        public async Task<StatsResponse> Stats() {
            if (!_settings.Cache.Database) return await _repo.GetStatsAsync(CancellationToken.None);

            return await _memoryCache.GetOrCreateAsync("stats", async cacheEntry => {
                cacheEntry.SetAbsoluteExpiration(new TimeSpan(0, 30, 0));

                return await _repo.GetStatsAsync(CancellationToken.None);
            });
        }
    }
}