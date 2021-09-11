using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Writers;
using OrderApi.Exceptions;
using OrderApi.Helpers;
using OrderApi.Interfaces;
using OrderApi.Models;
using OrderApi.Settings;
using OrderApi.Wrappers;

namespace OrderApi.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopClientService _clientService;
        private readonly RedisSettings _redisSettings;
        private readonly IServiceProvider _provider;
        public ShopService(IShopClientService clientService,
                 RedisSettings redisSettings, IServiceProvider serviceProvider)
        {
            _redisSettings = redisSettings;
            _provider = serviceProvider;
            _clientService = clientService;
        }
        public async Task<BaseResponse<ShopDetails>> GetShopDetailsAsync(Guid shopId)
        {
            return await _clientService.GetShopDetailsAsync(shopId);
        }

        public async Task<BaseResponse<IEnumerable<Shop>>> GetShopsAsync()
        {
            if (_redisSettings.Enabled)
            {
                var _distributedCache = GetCacheService();
                var resultFromCache = await _distributedCache.GetAsync($"{typeof(Shop).Name}_shops");
                if (resultFromCache != null)
                {
                    return new BaseResponse<IEnumerable<Shop>>(resultFromCache);
                }
            }
            var shops = await _clientService.GetShopsAsync();
            return shops is null? new BaseResponse<IEnumerable<Shop>>(null,"List of shops is empty",false) :shops;
        }

        private IDistributedCache<Shop> GetCacheService()
        {
            using var scope = _provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            return scope.ServiceProvider
                .GetRequiredService<IDistributedCache<Shop>>();
        } 
    }
}
