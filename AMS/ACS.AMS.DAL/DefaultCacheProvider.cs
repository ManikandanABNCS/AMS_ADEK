using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace ACS.AMS.DAL
{
    public class DefaultCacheProvider
    {
        public ObjectCache Cachedynamic { get { return MemoryCache.Default; } }
        public object Get(string key)
        {
            return Cachedynamic[key];
        }

        public void Set(string key, object data, int cacheTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);

            Cachedynamic.Add(new CacheItem(key, data), policy);
        }
        public bool IsSet(string key)
        {
            return (Cachedynamic[key] != null);
        }

        public void Invalidate(string key)
        {
            Cachedynamic.Remove(key);
        }
    }
}
