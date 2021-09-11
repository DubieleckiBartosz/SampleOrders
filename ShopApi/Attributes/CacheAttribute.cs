using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.Interfaces;
using ShopApi.Settings;

namespace ShopApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheAttribute:Attribute,IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        private readonly int _unusedExpireTime;
        private readonly string _key;
        public CacheAttribute(string key)
        {
            _key = key;
        }
        public CacheAttribute(int timeToLiveSeconds,string key):this(key)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public CacheAttribute(int timeToLiveSeconds, int unusedExpireTime,string key)
            : this(timeToLiveSeconds,key)
        {
            _unusedExpireTime = unusedExpireTime;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var settings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();
            if (!settings.Enabled)
            {
                await next();
                return;
            }
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var data =await cacheService.GetCacheAsync(_key);

            if(data is not null)
            {
                var result = new ContentResult
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = result;
                return;
            }
            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okObjectResult)
            {

                if (_timeToLiveSeconds == default && _unusedExpireTime == default)
                {
                    await cacheService.SetCacheAsync(_key, okObjectResult.Value);
                }
                else if (_timeToLiveSeconds != default && _unusedExpireTime == default)
                {
                    await cacheService.SetCacheAsync(_key, okObjectResult.Value,
                        TimeSpan.FromSeconds(_timeToLiveSeconds));
                }
                else if (_timeToLiveSeconds != default && _unusedExpireTime != default
                                                       && _timeToLiveSeconds > _unusedExpireTime)
                {
                    await cacheService.SetCacheAsync(_key, okObjectResult.Value,
                        TimeSpan.FromSeconds(_timeToLiveSeconds), TimeSpan.FromSeconds(_unusedExpireTime));
                }
                else
                {
                    throw new Exception("Incorrect time data has been entered ");
                }
            }
        }
    }
}
