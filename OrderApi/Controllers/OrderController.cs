using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Interfaces;
using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("{shopId}")]
        public async Task<IActionResult> CreateOrder([FromRoute]Guid shopId,[FromBody] CreateOrder order)
        {
            order.ShopId = shopId;
            return Ok(await _orderService.CreateOrderAsync(order));
        }
    }
}
