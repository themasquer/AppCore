using AppCore.Business.Abstracts.Utils;
using AppCore.Business.Abstracts.Utils.Security.Identity;

namespace AppCore.Business.Concretes.Utils.Security.Identity
{
    public class JwtUtil : JwtUtilBase
    {
        public JwtUtil(AppSettingsUtilBase appSettingsUtil) : base(appSettingsUtil)
        {
            
        }
    }
}
