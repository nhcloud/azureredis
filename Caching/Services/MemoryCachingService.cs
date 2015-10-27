using Caching.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using static System.DateTimeOffset;

namespace Caching.Services
{
    public class MemoryCachingService : ICachingService
    {
        private static readonly ObjectCache ObjectCache;
        private static readonly int CacheExpireInSeconds;

        static MemoryCachingService()
        {
            ObjectCache = new MemoryCache("company");
            CacheExpireInSeconds = 20;
            if (ConfigurationManager.AppSettings["company:CacheExpireInSeconds"] != null)
            {
                CacheExpireInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["company:CacheExpireInSeconds"]);
            }
        }

        public void Add(string key, object value)
        {
            ObjectCache.Add(key, value, Now.AddSeconds(CacheExpireInSeconds));
        }

        public void Add(string key, object value, TimeSpan? absoluteExpiration)
        {
            if (absoluteExpiration != null)
                ObjectCache.Add(key, value, Now.Add(absoluteExpiration.Value));
            else
                ObjectCache.Add(key, value, null);
        }

        public void ClearAll()
        {
            RemoveAll(null);
        }

        public object Get(string key)
        {
            return ObjectCache.Get(key);
        }

        public T Get<T>(string key)
        {
            return (T)ObjectCache.Get(key);
        }

        public void Remove(string key)
        {
            ObjectCache.Remove(key);
        }

        public void RemoveAll(string[] containsStrings)
        {
            var keys = new List<string>();
            var enumerator = (IDictionaryEnumerator)((IEnumerable)ObjectCache).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var key = enumerator.Key.ToString();
                if (containsStrings != null && containsStrings.Length > 0)
                {
                    if (containsStrings.Any(containsString => key.Contains(containsString)))
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
