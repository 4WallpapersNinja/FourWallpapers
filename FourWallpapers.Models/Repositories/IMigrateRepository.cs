using System.Threading;
using System.Threading.Tasks;

namespace FourWallpapers.Models.Repositories
{
    public interface IMigrateRepository
    {
        /// <summary>
        ///     Fully Initialize Database
        /// </summary>
        Task InitializeAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Run Only Databse Migrations
        /// </summary>
        Task MigrateAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Get Current Version of the Database
        /// </summary>
        /// <returns></returns>
        Task<int> CheckAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Cleans all data from the database only leaving empty tables
        /// </summary>
        Task CleanAsync(CancellationToken cancellationToken);
    }
}