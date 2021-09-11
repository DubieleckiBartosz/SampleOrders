using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.Interfaces;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        public ShopController(IShopService shopClientService)
        {
            _shopService = shopClientService;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAllShops()
        {
            var result = await _shopService.GetShopsAsync();
            return result.Data is null ? NotFound(result) : Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetShopDetails([FromRoute] Guid shopId)
        {
            var result = await _shopService.GetShopDetailsAsync(shopId);
            return result.Data is null ? NotFound(result) : Ok(result);
        }
    }
}
