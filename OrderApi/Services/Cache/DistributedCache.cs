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

        public async Task SetAsync(string key, IEnumerable<T> items, int minutesToCache,int slidingExpiration)
        {
            if (items.Any()) 
            {
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(minutesToCache))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration));
                var bytes = items.Serialize();
                await _distributedCache.SetAsync(key,bytes, options);
            }
        }
    }
}
