using AppCore.Business.Abstracts.Utils;
using AppCore.Business.Abstracts.Utils.Security.Identity;

namespace AppCore.Business.Concretes.Utils.Security.Identity
{
    public sealed class AccessTokenUtil : AccessTokenUtilBase
    {
        public AccessTokenUtil(AppSettingsUtilBase appSettingsUtil) : base(appSettingsUtil)
        {
            
        }
    }
}
