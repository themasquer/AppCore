using AppCore.MvcWebUI.Extensions;
using Microsoft.AspNetCore.Http;

namespace AppCore.Business.Abstracts.Utils
{
    public abstract class SessionUtilBase<T> where T : class 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected SessionUtilBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual void ClearSession(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }

        public virtual T GetSession(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetObject<T>(key);
        }

        public virtual void SetSession(string key, T t)
        {
            _httpContextAccessor.HttpContext.Session.SetObject(key, t);
        }
    }
}
