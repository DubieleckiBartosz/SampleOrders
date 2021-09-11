using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Background;

namespace OrderApi.ConfigExtensions
{
    public static class RegisterBackgroundService
    {
        public static IServiceCollection GetClientBackgroundService(this IServiceCollection services)
        {
            services.AddHostedService<BackgroundCacheClient>();
            return services;
        }
    }
}
