using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Dto.Orders;
using ShopApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApi.QueryParameters;
using Swashbuckle.AspNetCore.Annotations;

namespace ShopApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Create new order")]
        [HttpPost("{shopId}",Name ="CreateOrder")]
        public async Task<IActionResult> CreateNewOrder([FromRoute]Guid shopId,[FromBody] CreateOrderDto orderDto)
        {
            var result =await _orderService.CreateOrderAsync(shopId,orderDto);
            if(result is null || result.Success is false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Delete order")]
        [HttpDelete(Name ="DeleteOrder")]
        public async Task<IActionResult> DeleteOrder([FromBody]Guid orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);
            return result.Success is false ? NotFound(result) : NoContent();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get csv file with orders")]
        [HttpGet]
        public async Task<IActionResult> GetCsvFile([FromQuery]CsvQuery queryParameters)
        {
            var result = await _orderService.GetOrdersByDataAsync(queryParameters);
            return File(result.Body,result.ContentType,result.FileName);
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "E-mail confirmation")]
        [HttpGet("confirm-order")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string order)
        {
            var result = await _orderService.ConfirmOrderAsync(order);
            return Ok(result);
        }

    }
}
