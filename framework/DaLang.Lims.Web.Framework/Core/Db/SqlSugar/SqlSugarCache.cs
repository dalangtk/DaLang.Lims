using SqlSugar;
using System;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Cache;

namespace DaLang.Lims.Web.Framework.Core.Db.SqlSugar
{
    public class SqlSugarCache : ICacheService
    {
        private static readonly ICacheTool _cache = AppInfo.GetRequiredService<ICacheTool>(false);
        public void Add<V>(string key, V value)
        {
            _cache.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            _cache.Set(key, value, new TimeSpan(cacheDurationInSeconds));
        }

        public bool ContainsKey<V>(string key)
        {
            return _cache.Exists(key);
        }

        public V Get<V>(string key)
        {
            return _cache.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return _cache.Keys;
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (_cache.Exists(cacheKey))
            {
                try
                {
                    return _cache.Get<V>(cacheKey);
                }
                catch
                {
                    _cache.Del(cacheKey);
                }
            }
            var result = create.Invoke();

            _cache.Set(cacheKey, result, new TimeSpan(cacheDurationInSeconds));

            return result;
        }

        public void Remove<V>(string key)
        {
            _cache.Del(key);
        }
    }
}
