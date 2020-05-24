using AppCore.Business.Abstracts.Utils;
using Microsoft.Extensions.Configuration;

namespace AppCore.Business.Concretes.Utils
{
    public class AppSettingsUtil : AppSettingsUtilBase
    {
        public AppSettingsUtil(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
