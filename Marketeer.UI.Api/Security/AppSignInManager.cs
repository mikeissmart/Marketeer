using Marketeer.Core.Domain.Entities.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Marketeer.UI.Api.Security
{
    public class AppSignInManager : SignInManager<AppUser>
    {
        public AppSignInManager(AppUserManager userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<AppUser>> logger, IAuthenticationSchemeProvider schemes,
            IUserConfirmation<AppUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {

        }
    }
}
