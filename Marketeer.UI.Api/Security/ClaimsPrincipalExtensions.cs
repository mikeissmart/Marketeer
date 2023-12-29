using System.Security.Claims;

namespace Marketeer.UI.Api.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claims)
        {
            var value = claims.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

            if (value == null)
                throw new ArgumentNullException();

            return int.Parse(value);
        }
    }
}
