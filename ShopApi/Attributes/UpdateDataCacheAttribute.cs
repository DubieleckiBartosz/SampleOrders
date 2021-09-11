using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.Interfaces;

namespace ShopApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class)]
    public class UpdateDataCacheAttribute:Attribute,IAsyncActionFilter
    {
        private readonly string _key;
        public UpdateDataCacheAttribute(string key)
        {
            _key = key;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var shopService = context.HttpContext.RequestServices.GetRequiredService<IShopService>();
            var executedContext = await next();

            if (executedContext.Result is OkObjectResult)
            {
                var data = await shopService.GetAllShopsAsync();
                await service.UpdateCacheAsync(_key, data);
            }
            await next();
            return;
        }
    }
}
