using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using OrderApi.Helpers;
using OrderApi.Interfaces;

namespace OrderApi.Services.Cache
{
    public class DistributedCache<T> : IDistributedCache<T>
    {
        private readonly IDistributedCache _distributedCache;
        public DistributedCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<T>> GetAsync(string key)
        {
            var result = await _distributedCache.GetAsync(key);
            if (result is null)
            {
                return null;
            }

            return result.Deserialize<IEnumerable<T>>();
        }

        public async Task SetAsync(string key, T item, int minutesToCache,int slidingExpiration)
        {
            if (item != null)
            {
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(slidingExpiration))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                var bytes = item.Serialize();
                await _distributedCache.SetAsync(key,bytes, options);
            }
        }
    }
}
