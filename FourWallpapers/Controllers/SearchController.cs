using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;
using FourWallpapers.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FourWallpapers.Controllers {
    [Route("api/Search")]
    public class SearchController : Controller {
        private readonly ISearchRepository _repo;
        private readonly IGlobalSettings _settings;
        private readonly IMemoryCache _memoryCache;

        public SearchController(IGlobalSettings globalSettings, ISearchRepository repo, IMemoryCache memoryCache) {
            _repo = repo;
            _memoryCache = memoryCache;
            _settings = globalSettings;
        }

        // POST api/search
        [HttpPost]
        public async Task<List<SearchResult>> Post([FromBody] SearchRequest request) {
            if (request == null) return new List<SearchResult>();

            if (!_settings.Cache.Database) return await _repo.SearchAsync(request, CancellationToken.None);

            string key;
            using (var md5 = System.Security.Cryptography.MD5.Create()) {
                //calculate key
                key = Core.Helpers.ByteToString(
                    md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.ToJson())));
            }

            return await _memoryCache.GetOrCreateAsync($"search:{key}", async cacheEntry => {
                cacheEntry.SetAbsoluteExpiration(new TimeSpan(0, 5, 0));

                return await _repo.SearchAsync(request, CancellationToken.None);
            });
        }

        [HttpGet]
        [Route("random")]
        public async Task<List<SearchResult>> Random() {
            return await _repo.GetRandomAsync(CancellationToken.None);
        }
    }
}