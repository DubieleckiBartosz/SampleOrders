using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OrderApi.Helpers;
using OrderApi.Interfaces;
using OrderApi.Models;
using OrderApi.Settings;


namespace OrderApi.Background
{
    public class BackgroundCacheClient:BackgroundService
    {
    private readonly IDistributedCache<Shop> _distributedCache;
        private readonly IShopClientService _shopClientService;
        private int _refreshCacheInSeconds;
        private readonly RedisSettings _reddiSettings;
        private int _timeRedis;
        public BackgroundCacheClient(IDistributedCache<Shop> distributedCache,
            IShopClientService shopClientService, RedisSettings settings)
        {
            _reddiSettings = settings;
            _shopClientService = shopClientService;
            _distributedCache = distributedCache;
            _timeRedis = _reddiSettings.TimeInMinutes;
            _refreshCacheInSeconds = _timeRedis > 1 ? (_timeRedis - 1) * 60 : 45;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var shops = await _shopClientService.GetShopsAsync(stoppingToken);
                    if (shops is object)
                    {
                        await _distributedCache.SetAsync($"{typeof(Shop).Name}_shops",
                            shops.Data, _timeRedis, 1);
                    }
                    
                await Task.Delay(TimeSpan.FromSeconds(_refreshCacheInSeconds), stoppingToken);
            }
        }
    }
}
