using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AppCore.Business.Abstracts.Helpers.Security.Identity
{
    public abstract class SecurityKeyHelperBase
    {
        public virtual SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
