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

        public virtual void BindAppSettings<T>(string sectionKey) where T : class, new()
        {
            var section = _configuration.GetSection(sectionKey);
            if (section != null)
            {
                var appSettings = new T();
                section.Bind(appSettings);
            }
        }
    }
}
