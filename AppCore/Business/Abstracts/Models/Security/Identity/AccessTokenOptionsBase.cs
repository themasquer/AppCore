namespace AppCore.Business.Abstracts.Models.Security.Identity
{
    public abstract class AccessTokenOptionsBase
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public string SecurityKey { get; set; }
    }
}
