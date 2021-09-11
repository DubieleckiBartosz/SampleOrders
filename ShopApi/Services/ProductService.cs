using ShopApi.Dto.Products;
using ShopApi.Interfaces;
using ShopApi.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopApi.AppDatabase;
using ShopApi.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace ShopApi.Services
{
    public class ProductService : BaseContextService<Product>, IProductService
    {
        private readonly IMapper _mapper;
        public ProductService(ApplicationDbContext db,IMapper mapper):base(db)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetShopProductsAsync(Guid shopId)
        {

            var result = await _db.Products.Where(w => w.ShopId == shopId).ToListAsync();
                if (result.Any())
                {
                    var resultMap = _mapper.Map<IEnumerable<ProductDto>>(result);
                    return BaseResponse<IEnumerable<ProductDto>>.Ok(resultMap);
                }
                return BaseResponse<IEnumerable<ProductDto>>.Error(BaseResponseStrings.ProductsNotFound);
        }

        public async Task<BaseResponse<ProductDto>> CreateNewProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var result = await _db.AddAsync(product);
            await _db.SaveChangesAsync();
            var productMap = _mapper.Map<ProductDto>(result.Entity);
            return BaseResponse<ProductDto>.Ok(productMap, BaseResponseStrings.RequestSuccess);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product is not null)
                {
                    _db.Products.Remove(product);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
        }

        public async Task<BaseResponse<string>> UpdateProductAsync(Guid productId, JsonPatchDocument<UpdateProductDto> patchDocument)
        {
            var product = await _db.Products.Where(w => w.Id == productId)
                .FirstOrDefaultAsync();
            if (product is null)
            {
                BaseResponse<string>.Error(BaseResponseStrings.DataNotFound(productId));
            }

            var prodToPatch = _mapper.Map<UpdateProductDto>(product);
            patchDocument.ApplyTo(prodToPatch);
            _mapper.Map(prodToPatch, product);
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return BaseResponse<string>.Ok(BaseResponseStrings.RequestSuccess);
        }
    }
}
