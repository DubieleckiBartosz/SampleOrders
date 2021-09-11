using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.ConfigExtensions
{
    public static class ConfigRabbitMq
    {
        public static IServiceCollection GetRabbitMq(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<RabbitSettings>(configuration.GetSection("RabbitSettings"));
            return services;
        }
    }
}
