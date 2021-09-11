using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using ShopApi.Dto.Products;
using ShopApi.Email;
using ShopApi.Filters;
using ShopApi.Interfaces;
using ShopApi.Models;
using ShopApi.Wrappers;
using Swashbuckle.AspNetCore.Annotations;

namespace ShopApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService,IEmailService emailService)
        {
            _productService = productService;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get all products by shopID")]
        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetProducts([FromRoute] Guid shopId)
        {
            var result = await _productService.GetShopProductsAsync(shopId);
            return result.Success is true ? Ok(result) : NotFound(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary ="Create new product")]
        [ServiceFilter(typeof(CreateProductValidateFilter))]
        [HttpPost("{shopId}")]
        public async Task<IActionResult> CreateProduct([FromRoute] Guid shopId,[FromBody] CreateProductDto productDto)
        {
            productDto.ShopId = shopId;
            var result = await _productService.CreateNewProductAsync(productDto);
            return result.Success is true ? Ok(result) : NotFound(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Update product")]
        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId,
            [FromBody] JsonPatchDocument<UpdateProductDto> patchDocument)
        {
            var result = await _productService.UpdateProductAsync(productId, patchDocument);
            return result.Success is true ? Ok(result) : NotFound(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Delete product")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var result = await _productService.DeleteProductAsync(productId);
            return result is true
                ? NoContent()
                : NotFound(BaseResponse<string>.Error(BaseResponseStrings.DataNotFound(productId)));
        }
    }
}
