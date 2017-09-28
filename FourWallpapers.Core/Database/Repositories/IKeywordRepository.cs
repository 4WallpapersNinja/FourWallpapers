using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core.Database.Entities;

namespace FourWallpapers.Core.Database.Repositories
{
    public interface IKeywordRepository : IRepository<Keyword>
    {
        Task LinkImageToKeywordAsync(decimal imageKey, string keyword, CancellationToken cancellationToken);
        Task<decimal?> FindKeywordKeyAsync(string keyword, CancellationToken cancellationToken);
        Task<bool> DoesLinkExistAsync(decimal imageKey, decimal keywordKey, CancellationToken cancellationToken);
    }
}