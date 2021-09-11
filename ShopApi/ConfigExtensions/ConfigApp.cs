using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.AppDatabase;
using ShopApi.Interfaces;
using ShopApi.Services;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using ShopApi.Background;
using ShopApi.Dto.OrderLines;
using ShopApi.Dto.Orders;
using ShopApi.Dto.Products;
using ShopApi.Dto.ShopDTO;
using ShopApi.Email;
using ShopApi.QueryParameters;
using ShopApi.Services.Cache;
using ShopApi.Settings;
using ShopApi.Validators;

namespace ShopApi.ConfigExtensions
{
    public static class ConfigApp
    {
        public static IServiceCollection GetAutoMapper(this IServiceCollection services)
        {
            services.AddHostedService<ConsumeOrderRabbitMq>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
        public static IServiceCollection GetDataBase(this IServiceCollection services,IConfiguration configuration)=>
             services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static IServiceCollection GetDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShopService, ShopService>();
            services.AddTransient<ICsvExporter, CsvExporter>();
            return services;
        }

        public static IServiceCollection GetValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateOrderDto>, CreateNewOrderDtoValidator>();
            services.AddTransient<IValidator<CreateShopDto>, CreateNewShopDtoValidator>();
            services.AddTransient<IValidator<CreateOrderLineDto>, CreateOrderLineDtoValidator>();
            services.AddTransient<IValidator<CsvQuery>, CsvQueryValidator>();
            return services;
        }

        public static IServiceCollection GetVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(v =>
            {
                v.DefaultApiVersion = new ApiVersion(1, 0);
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.ReportApiVersions = true;
                v.ApiVersionReader = new MediaTypeApiVersionReader("v");
            });
            return services;
        }

        public static IServiceCollection GetRedisConfig(this IServiceCollection services,string connection)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connection;
                options.InstanceName = "Shop_App";
            });
            services.AddSingleton<ICacheService,CacheService>();
            return services;
        }
        public static IServiceCollection GetAccessor(this IServiceCollection services)=>
            services.AddHttpContextAccessor();

        public static IServiceCollection GetEmailService(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService,EmailService>();
            return services;
        }
    }
}
