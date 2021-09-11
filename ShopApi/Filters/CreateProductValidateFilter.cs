using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopApi.AppDatabase;
using ShopApi.Dto.Products;
using ShopApi.Entities;
using ShopApi.Interfaces;
using ShopApi.Wrappers;

namespace ShopApi.Filters
{
    public class CreateProductValidateFilter:IAsyncActionFilter
    {
        private readonly ApplicationDbContext _db;
        public CreateProductValidateFilter(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guid id = Guid.Empty;
            if (context.ActionArguments.ContainsKey("shopId"))
            {
                id = (Guid) context.ActionArguments["shopId"];
            }
            else
            {
                context.Result = new BadRequestObjectResult(
                    BaseResponse<string>.Error("Bad id parameter"));
                return;
            }

            var shop = await _db.Shops.Where(w => w.Id == id)
                .Include(q => q.Products).FirstOrDefaultAsync();
            if (shop is null)
            {
                context.Result = new BadRequestObjectResult(
                    BaseResponse<string>.Error("Bad id parameter"));
                return;
            }

            var createProduct = context.ActionArguments["productDto"] as CreateProductDto;
            if (createProduct.Quantity == 0 || createProduct.Price == 0)
            {
                context.Result = new BadRequestObjectResult(
                    BaseResponse<string>.Error("Product must have a price"));
                return;
            }
            if (shop.Products.Any())
            {
                if (shop.Products.Any(q => q.Name.Contains(createProduct.Name)))
                {
                    context.Result = new BadRequestObjectResult(
                        BaseResponse<string>.Error("This product exists in your shop. You can update its quantity"));
                    return;
                }
            }
            await next();
        }
    }
}
