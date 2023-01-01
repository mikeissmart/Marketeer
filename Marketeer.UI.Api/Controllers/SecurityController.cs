using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Security;
using Marketeer.UI.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly AppSignInManager _signInManager;
        private readonly AppUserManager _userManager;
        private readonly JwtConfig _jwtConfig;

        public SecurityController(AppUserManager userManager,
            AppSignInManager signInManager,
            JwtConfig jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto modelDto)
        {
            var user = await _userManager.FindByNameAsync(modelDto.UserName);
            if (user == null)
                return Unauthorized();

            var signIn = await _signInManager.PasswordSignInAsync(user, modelDto.Password, true, false);
            if (!signIn.Succeeded)
                return Unauthorized();

            var roles = (await _userManager.GetRolesAsync(user)).ToArray();
            var secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor()
            {
                Audience = "",
                Issuer = "",
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Roles", string.Join(",", roles))
                })
            };

            return Ok(new StringDataDto()
            {
                Data = handler.WriteToken(handler.CreateToken(descriptor))
            });
        }
    }
}
