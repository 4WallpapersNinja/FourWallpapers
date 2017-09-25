using FourWallpapers.Core.Settings;
using FourWallpapers.Models;
using FourWallpapers.Models.Repositories;

namespace FourWallpapers.Repositories.SqlServer
{
    public class UserServerRepository : BaseSqlServerRepository<User>, IUserRepository
    {
        public UserServerRepository(IDatabaseSettings db) : base(db)
        {
        }
    }
}