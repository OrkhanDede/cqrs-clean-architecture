namespace Core.Models
{
    public class TokenSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireMinutes { get; set; }
        public int RefreshTokenExpireMinutes { get; set; }
        public int RememberMeRefreshTokenExpireDays { get; set; }
    }
}