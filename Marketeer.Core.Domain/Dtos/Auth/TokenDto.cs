using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Auth
{
    public class TokenDto : IRefactorType
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
