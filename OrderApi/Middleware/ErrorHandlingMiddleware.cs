using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OrderApi.Exceptions;
using OrderApi.Wrappers;

namespace OrderApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    ApiClientException e => e.StatusCode,
                    BadRequestException e => e.StatusCode,
                    NotFoundException e => e.StatusCode,
                    OrderAppException e => e.StatusCode,
                    _ => (int)HttpStatusCode.InternalServerError,
                };
                await response.WriteAsJsonAsync(new BaseResponse<string>(default, error?.Message, false));
            }
        }
    }
}
