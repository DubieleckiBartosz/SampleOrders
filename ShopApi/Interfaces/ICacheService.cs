using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Interfaces
{
    public interface ICacheService
    {
        Task<string> GetCacheAsync(string key);
        Task SetCacheAsync(string key, object data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
        Task UpdateCacheAsync(string key, object data);
        Task DeleteAsync(string key);
    }
}
