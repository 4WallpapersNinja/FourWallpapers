using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Models.Requests;
using FourWallpapers.Models.Responses;

namespace FourWallpapers.Models.Repositories
{
    public interface ISearchRepository : IRepository<Image>
    {
        Task<List<SearchResult>> SearchAsync(SearchRequest request, CancellationToken cancellationToken);

        Task<List<SearchResult>> TopImageSearchAsync(TopRequest updateRequest, CancellationToken cancellationToken);

        Task<List<SearchResult>> GetRecentAsync(CancellationToken cancellationToken);

        Task<StatsResponse> GetStatsAsync(CancellationToken cancellationToken);

        Task<List<SearchResult>> GetRandomAsync(CancellationToken cancellationToken);
    }
}