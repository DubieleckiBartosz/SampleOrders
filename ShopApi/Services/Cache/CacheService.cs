using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ShopApi.Helpers;
using ShopApi.Interfaces;

namespace ShopApi.Services.Cache
{
    public class CacheService: ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<string> GetCacheAsync(string key)
        {
            var data = await _distributedCache.GetStringAsync(key);
            if (data != null)
            {
                return data;
            }
            return default;
        }

        public async Task SetCacheAsync(string key, object data, 
            TimeSpan? absoluteExpireTime = null, 
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ??
                                                      TimeSpan.FromSeconds(60 * 20);
            options.SlidingExpiration = unusedExpireTime ?? TimeSpan.FromSeconds(60);
            var dataSerialize = JsonConvert.SerializeObject(data);
            await _distributedCache.SetStringAsync(key, dataSerialize, options);
        }
        public async Task UpdateCacheAsync(string key, object data)
        {
            var dataSerialize = JsonConvert.SerializeObject(data);
            await _distributedCache.SetStringAsync(key, dataSerialize);
        }
        public async Task DeleteAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

    }
}
