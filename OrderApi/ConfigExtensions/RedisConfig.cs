using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Interfaces;
using OrderApi.Services.Cache;

namespace OrderApi.ConfigExtensions
{
    public static class RedisConfig
    {
        public static IServiceCollection GetRedis(this IServiceCollection services,string connection)
        {
            services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connection;
                options.InstanceName = "ClientShop_App";
            });
            return services;
        }
    }
}
