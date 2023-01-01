using System.ComponentModel.DataAnnotations;

namespace Marketeer.Core.Domain.Dtos.Security
{
    public class LoginDto : IRefactorType
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
