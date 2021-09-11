using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ShopApi.Attributes;
using ShopApi.Dto.ShopDTO;
using ShopApi.Interfaces;
using ShopApi.Wrappers;
using Swashbuckle.AspNetCore.Annotations;

namespace ShopApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get all shops with city name")]
        [Cache("List_Shops")]
        [HttpGet]
        public async Task<IActionResult> GetAllShops()
        {
            var result = await _shopService.GetAllShopsAsync();
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get shop by ID")]
        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetShopById([FromRoute]Guid shopId)
        {
            var result = await _shopService.GetDetailsShopAsync(shopId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create shop")]
        [UpdateDataCache("List_Shops")]
        [HttpPost]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopDto createShop)
        {
            var result = await _shopService.CreateShopAsync(createShop);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Update shop")]
        [UpdateDataCache("List_Shops")]
        [HttpPut("{shopId}")]
        public async Task<IActionResult> UpdateShop([FromRoute] Guid shopId, [FromBody] UpdateShopDto shopDto)
        {
            if (shopDto is null)
            {
                return BadRequest(BaseResponse<ShopDto>.Error(BaseResponseStrings.ShopNull));
            }

            var result = await _shopService.UpdateShopAsync(shopId, shopDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Delete shop")]
        [UpdateDataCache("List_Shops")]
        [HttpDelete("{shopId}")]
        public async Task<IActionResult> DeleteShop([FromRoute] Guid shopId)
        {
            var result = await _shopService.DeleteShopAsync(shopId);
            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

    }
}
