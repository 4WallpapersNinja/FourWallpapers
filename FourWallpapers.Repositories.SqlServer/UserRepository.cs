using System.Threading.Tasks;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Core.Models.Request;
using FourWallpapers.Core.Settings;

namespace FourWallpapers.Repositories.SqlServer
{
    public class UserRepository : BaseSqlServerRepository<User>, IUserRepository
    {

        public UserRepository(IDatabaseSettings db) : base(db)
        {
            InitializeQueries();
        }
        public UserRepository(string connectionString) : base(connectionString)
        {
            InitializeQueries();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> SignInAsync(Login login)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> SignInTwoFactorAsync(LoginWithTwoFactor login)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GenerateEmailConfirmationTokenAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CheckPasswordSignInAsync(User user, string password)
        {
            throw new System.NotImplementedException();
        }

        private void InitializeQueries()
        {
            //_findAllQuery = SqlServerUtilities.GetQuery("Image.FindAll");
            //_findByIdQuery = SqlServerUtilities.GetQuery("Image.FindById");
            //_incrementAndFindByIdQuery = SqlServerUtilities.GetQuery("Image.Increment") + _findByIdQuery;
            //_insertQuery = SqlServerUtilities.GetQuery("Image.Insert");
        }
    }
}