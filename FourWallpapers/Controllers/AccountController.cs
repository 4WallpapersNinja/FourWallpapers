using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Core.Database.Repositories;
using FourWallpapers.Core.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace FourWallpapers.Controllers
{
    [Authorize(Policy = "AuthorizeJwt")]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AccountController(
            IUserRepository userRepository,
            ILoggerFactory loggerFactory,
            IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }
        //public async Task<IActionResult> Register([FromBody])
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new User {Username = model.Email, Email = model.Email};
                var result = await _userRepository.CreateAsync(user, model.Password);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody] Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userRepository.CheckPasswordSignInAsync(user, model.Password);
                    if (result)
                    {
                        string shaHash = Helpers.Auth.HashUser(user);
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("AuthHash", shaHash)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                            _config["Tokens:Issuer"],
                            claims,
                            expires: DateTime.Now.AddDays(7),
                            signingCredentials: creds);

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                }
            }

            return BadRequest("Could not create token");
        }
    }
}