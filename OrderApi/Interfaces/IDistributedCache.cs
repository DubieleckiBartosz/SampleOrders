using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace OrderApi.Interfaces
{
    public interface IDistributedCache<T>
    {
        Task<IEnumerable<T>> GetAsync(string key);
        Task SetAsync(string key, IEnumerable<T> items, int minutesToCache, int slidingExpiration);
    }
}
