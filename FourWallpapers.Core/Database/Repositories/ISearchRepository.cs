using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Models.Request;
using FourWallpapers.Core.Models.Response;

namespace FourWallpapers.Core.Database.Repositories
{
    public interface ISearchRepository : IRepository<Image>
    {
        Task<List<SearchResult>> SearchAsync(Search request, CancellationToken cancellationToken);

        Task<List<SearchResult>> TopImageSearchAsync(Top updateRequest, CancellationToken cancellationToken);

        Task<List<SearchResult>> GetRecentAsync(CancellationToken cancellationToken);

        Task<StatsResponse> GetStatsAsync(CancellationToken cancellationToken);

        Task<List<SearchResult>> GetRandomAsync(CancellationToken cancellationToken);
    }
}