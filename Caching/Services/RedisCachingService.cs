using Caching.Common;
using Caching.Interfaces;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Linq;

namespace Caching.Services
{
    public class RedisCachingService : ICachingService
    {
        private static readonly int CacheExpireInSeconds;

        static RedisCachingService()
        {
            CacheExpireInSeconds = 20;
            if (ConfigurationManager.AppSettings["company:CacheExpireInSeconds"] != null)
            {
                CacheExpireInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["company:CacheExpireInSeconds"]);
            }
        }

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["company:RedisConnection"]));

        //Using ConfigOptions
        //public static readonly Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    var config = new ConfigurationOptions();
        //    config.EndPoints.Add("nashuaug.redis.cache.windows.net");
        //    config.Password = "f7CW+W2uzhFKKlTrO1jANth/N6QRDnG4uxa12MVtOXc=";
        //    config.Ssl = true;
        //    config.AbortOnConnectFail = false;
        //    config.ConnectTimeout = 1000;
        //    return ConnectionMultiplexer.Connect(config);
        //});

        private static ConnectionMultiplexer Connection => LazyConnection.Value;

        public void Add(string key, object value)
        {
            Connection.GetDatabase().StringSet(key, Serializer.Serialize(value), TimeSpan.FromSeconds(CacheExpireInSeconds));
        }

        public void Add(string key, object value, TimeSpan? absoluteExpiration)
        {
            if (absoluteExpiration != null)
                Connection.GetDatabase().StringSet(key, Serializer.Serialize(value), TimeSpan.FromSeconds(absoluteExpiration.Value.Seconds));
            else
                Connection.GetDatabase().StringSet(key, Serializer.Serialize(value), null);
        }

        public void ClearAll()
        {
            RemoveAll(null);
        }

        public object Get(string key)
        {
            return Serializer.Deserialize<object>(Connection.GetDatabase().StringGet(key));
        }

        public T Get<T>(string key)
        {
            return Serializer.Deserialize<T>(Connection.GetDatabase().StringGet(key));
        }

        public void Remove(string key)
        {
            Connection.GetDatabase().KeyDelete(key);
        }

        public void RemoveAll(string[] containsStrings)
        {
            var endpoints = Connection.GetEndPoints();
            var server = Connection.GetServer(endpoints[0]);
            var keys = server.Keys();
            foreach (var key in keys)
            {
                if (containsStrings != null && containsStrings.Length > 0)
                {
                    if (containsStrings.Any(containsString => key.ToString().Contains(containsString)))
                        Remove(key);
                }
                else
                {
                    Remove(key);
                }
            }
        }
    }
}