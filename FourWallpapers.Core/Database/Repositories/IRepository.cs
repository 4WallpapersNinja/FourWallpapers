using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FourWallpapers.Core.Database.Repositories
{
    public interface IRepository<T>
    {
        Task<Guid> AddAsync(T item, CancellationToken cancellationToken);
        Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(T item, CancellationToken cancellationToken);
        Task<T> FindByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken);
    }
}