using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Text;

namespace AppCore.MvcWebUI.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value) where T : class 
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(key))
                {
                    string serializedValue = JsonConvert.SerializeObject(value); 
                    byte[] bytes = Encoding.UTF8.GetBytes(serializedValue);
                    session.Set(key, bytes);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static T GetObject<T>(this ISession session, string key) where T : class 
        {
            try
            {
                if (String.IsNullOrWhiteSpace(key))
                    return null;
                byte[] bytes;
                session.TryGetValue(key, out bytes);
                if (bytes == null)
                    return null;
                string deserializedValue = Encoding.UTF8.GetString(bytes);
                T value = JsonConvert.DeserializeObject<T>(deserializedValue);
                return value;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
