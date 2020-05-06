using Microsoft.Extensions.Configuration;

namespace AppCore.Business.Abstracts.Utils
{
    public abstract class AppSettingsUtilBase
    {
        private readonly IConfiguration _configuration;

        protected AppSettingsUtilBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual T BindAppSettings<T>(string sectionKey = "AppSettings") where T : class, new()
        {
            T t = null;
            var section = _configuration.GetSection(sectionKey);
            if (section != null)
            {
                t = new T();
                section.Bind(t);
            }
            return t;
        }
    }
}
