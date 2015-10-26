using Caching.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Caching.Services
{
    public class WebCachingService : ICachingService
    {
        private static readonly int CacheExpireInSeconds;

        static WebCachingService()
        {
            CacheExpireInSeconds = 20;
            if (ConfigurationManager.AppSettings["company:CacheExpireInSeconds"] != null)
            {
                CacheExpireInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["company:CacheExpireInSeconds"]);
            }
        }

        public void Add(string key, object value)
        {
            Add(key, value, TimeSpan.FromSeconds(CacheExpireInSeconds));
        }

        public void Add(string key, object value, TimeSpan? absoluteExpiration)
        {
            if (HttpContext.Current == null) return;
            HttpContext.Current.Cache.Add(key, value, null,
                absoluteExpiration != null
                    ? DateTime.Now.AddSeconds(absoluteExpiration.Value.Seconds)
                    : DateTime.Now.AddDays(365),
                Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        public void ClearAll()
        {
            RemoveAll(null);
        }

        public object Get(string key)
        {
            object val = null;

            if (HttpContext.Current != null)
            {
                val = HttpContext.Current.Cache.Get(key);
            }

            return val;
        }

        public T Get<T>(string key)
        {
            var val = default(T);

            if (HttpContext.Current != null)
            {
                val = (T)HttpContext.Current.Cache.Get(key);
            }

            return val;
        }

        public void Remove(string key)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Remove(key);
            }
        }

        public void RemoveAll(string[] containsStrings)
        {
            if (HttpContext.Current == null) return;
            var keys = new List<string>();
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var key = enumerator.Key.ToString();

                if (containsStrings != null && containsStrings.Length > 0)
                {
                    if (containsStrings.Any(containsString => key.ToString().Contains(containsString)))
                        keys.Add(key);
                }
                else
                {
                    keys.Add(key);
                }
            }
            foreach (var key in keys)
            {
                Remove(key);
            }
        }
    }
}