namespace Marketeer.Core.Domain.Dtos.Security
{
    public class ClientTokenDto : IRefactorType
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Exp { get; set; }
        public DateTime Iat { get; set; }
        public DateTime Nbf { get; set; }
        public List<string> Roles { get; set; }
    }
}
