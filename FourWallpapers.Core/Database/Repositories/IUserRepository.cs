using System.Threading.Tasks;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Models.Request;

namespace FourWallpapers.Core.Database.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByEmailAsync(string email);
        Task<User> SignInAsync(Login login);
        Task<User> SignInTwoFactorAsync(LoginWithTwoFactor login);
        Task<User> GenerateEmailConfirmationTokenAsync(User user);
        Task<User> CreateAsync(User user, string password);
        Task<bool> CheckPasswordSignInAsync(User user, string password);
    }
}