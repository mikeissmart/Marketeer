using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Marketeer.Core.Domain.Entities.Auth
{
    public class AppUser : IdentityUser<int>
    {
        [StringLength(44)]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
    }
}