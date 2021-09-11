using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.Interfaces;
using OrderApi.Services;

namespace OrderApi.ConfigExtensions
{
    public static class ConfigHttpClient
    {
        public static IServiceCollection GetHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IShopClientService,ShopClientService>(client =>
            {
                client.BaseAddress =new Uri("https://localhost:5002/");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            });
            return services;
        }
    }
}
