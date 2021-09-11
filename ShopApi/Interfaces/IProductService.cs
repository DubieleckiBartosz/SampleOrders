using ShopApi.Dto.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ShopApi.Wrappers;
using Microsoft.AspNetCore.JsonPatch;

namespace ShopApi.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponse<IEnumerable<ProductDto>>> GetShopProductsAsync(Guid shopId);
        Task<BaseResponse<ProductDto>> CreateNewProductAsync(CreateProductDto productDto);
        Task<BaseResponse<string>> UpdateProductAsync(Guid productId,JsonPatchDocument<UpdateProductDto> patchDocument);
        Task<bool> DeleteProductAsync(Guid id);
    }
}
