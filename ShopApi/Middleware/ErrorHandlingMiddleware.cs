using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopApi.Exceptions;
using ShopApi.Wrappers;

namespace ShopApi.Middleware
{
    public class ErrorHandlingMiddleware:IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(ShopException ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)ex.StatusCode;
                _logger.LogError(ex?.Message);
                await response.WriteAsJsonAsync(new BaseResponse<string>(default, ex.Message, false));
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(ex?.Message);
                await response.WriteAsJsonAsync(new BaseResponse<string>(default, "Something went wrong", false));
            }
        }
    }
}
