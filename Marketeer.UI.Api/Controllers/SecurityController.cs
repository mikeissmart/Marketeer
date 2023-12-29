using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Auth;
using Marketeer.Core.Domain.Dtos.Security;
using Marketeer.Core.Service.Auth;
using Marketeer.Persistance.Database.Repositories.Auth;
using Marketeer.UI.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SecurityController : ControllerBase
    {
        private readonly AppSignInManager _signInManager;
        private readonly AppUserManager _userManager;
        private readonly ITokenService _tokenService;
        private readonly IAppUserRepository _appUserRepository;

        public SecurityController(AppUserManager userManager,
            AppSignInManager signInManager,
            ITokenService tokenService,
            IAppUserRepository appUserRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _appUserRepository = appUserRepository;
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

            _tokenService.GenerateRefreshToken(out var refreshToken, out var refreshExpires);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpires = refreshExpires;
            _appUserRepository.Update(user);
            await _appUserRepository.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new TokenDto
            {
                AccessToken = _tokenService.GenerateAccessToken(user, roles),
                RefreshToken = refreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(TokenDto token)
        {
            if (token == null)
                return BadRequest("Invalid client request");

            var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Claims.First(x => x.Type == "UserName").Value; //this is mapped to the Name claim by default

            var user = await _appUserRepository.GetUserByUserNameAsync(username);
            if (user == null ||
                user.RefreshToken != token.RefreshToken ||
                user.RefreshTokenExpires <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            _tokenService.GenerateRefreshToken(out var refreshToken, out var refreshExpires);

            user.RefreshToken = refreshToken;
            _appUserRepository.Update(user);
            await _appUserRepository.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new TokenDto
            {
                AccessToken = _tokenService.GenerateAccessToken(user, roles),
                RefreshToken = refreshToken
            });
        }

        [HttpPost("Revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity!.Name!;
            var user = await _appUserRepository.GetUserByUserNameAsync(username);
            if (user == null)
                return BadRequest();

            user.RefreshToken = null;
            user.RefreshTokenExpires = null;
            _appUserRepository.Update(user);
            await _appUserRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
