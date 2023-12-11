namespace Marketeer.Common.Configs
{
    public class JwtConfig : IConfig
    {
        public string Secret { get; set; }
        public int TokenExpiresInMinutes { get; set; } = 10;
        public int RefreshTokenExpiresInDays { get; set; } = 7;
    }
}
