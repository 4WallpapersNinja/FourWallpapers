using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FourWallpapers.Helpers
{
    public static class Auth
    {
        public static string HashUser(User user)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return Core.Helpers.Utilities.ByteToString(sha256.ComputeHash(Encoding.UTF8.GetBytes($"{user.Email}-{user.Id}-{user.PasswordHash}")));
            }
        }
    }

    // A handler that can determine whether a MaximumOfficeNumberRequirement is satisfied
    internal class AuthorizedTokenHandler : AuthorizationHandler<AuthorizedTokeRequirement>
    {
        private readonly IUserRepository _userManager;

        public AuthorizedTokenHandler(IUserRepository userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizedTokeRequirement requirement)
        {
            // Bail out if the office number claim isn't present
            if (!context.User.HasClaim(c => c.Type == "AuthHash") || !context.User.HasClaim(c => c.Type == JwtRegisteredClaimNames.Sub))
            {
                return Task.CompletedTask;
            }

            // Bail out if we can't read a string
            string authHash = context.User.FindFirst(c => c.Type == "AuthHash").Value;
            if (string.IsNullOrWhiteSpace(authHash))
            {
                return Task.CompletedTask;
            }

            //get the user
            var user = _userManager.FindByEmailAsync(context.User.FindFirst(c => c.Type == JwtRegisteredClaimNames.Sub).Value).GetAwaiter().GetResult();

            // Finally, validate that the hash matches what the claim has
            if (authHash == Helpers.Auth.HashUser(user))
            {
                // Mark the requirement as satisfied
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    // A custom authorization requirement which requires office number to be below a certain value
    internal class AuthorizedTokeRequirement : IAuthorizationRequirement
    {
    }
}
