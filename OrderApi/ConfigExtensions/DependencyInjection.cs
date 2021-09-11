using Microsoft.Extensions.DependencyInjection;
using OrderApi.Interfaces;
using OrderApi.Rabbit;
using OrderApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.Services.Cache;

namespace OrderApi.ConfigExtensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection GetDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IShopService, ShopService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<IRabbitManager, RabbitManager>();
            services.AddSingleton<RabbitConnection>();
            return services;
        } 
    }
}
