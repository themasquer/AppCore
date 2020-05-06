using Microsoft.IdentityModel.Tokens;

namespace AppCore.Business.Abstracts.Helpers.Security.Identity
{
    public abstract class SigningCredentialsHelperBase
    {
        public virtual SigningCredentials CreateSigningCredentials(SecurityKey securityKey, string securityAlgorithm = SecurityAlgorithms.HmacSha256Signature)
        {
            return new SigningCredentials(securityKey, securityAlgorithm);
        }
    }
}
