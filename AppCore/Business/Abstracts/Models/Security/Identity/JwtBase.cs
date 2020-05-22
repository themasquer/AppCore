using System;

namespace AppCore.Business.Abstracts.Models.Security.Identity
{
    public abstract class JwtBase
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
