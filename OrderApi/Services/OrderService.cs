using System;
using OrderApi.Interfaces;
using OrderApi.Models;
using OrderApi.Wrappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.Exceptions;

namespace OrderApi.Services
{
    public class OrderService:IOrderService
    {
        private Dictionary<string, object> _headers;
        private readonly IRabbitManager _rabbitManager;
        private readonly IShopService _shopService;
        public OrderService(IRabbitManager rabbitManager,
            IShopService shopService)
        {
            _shopService = shopService;
            _rabbitManager = rabbitManager;
            _headers = new();
        }

        public async Task<BaseResponse<string>> CreateOrderAsync(CreateOrder order)
        {
            var shopService = await _shopService.GetShopsAsync();
            if (shopService.Data == null)
            {
                throw new ApiClientException("sorry but data is temporarily unavailable, please try again later",418);
            }
            var shopExist =shopService.Data.Any(w => w.Id.Equals(order.ShopId));
            if (!shopExist)
            {
                throw new NotFoundException($"Shop: {order.ShopId.ToString()} not found", 404);
            } 
            if (order is null)
            {
                throw new ArgumentNullException("Order is null");
            }
            if (string.IsNullOrEmpty(order.Address))
            {
                throw new BadRequestException("you need to enter the address in the order",400);
            }
            if (!order.Line.Any())
            {
                throw new BadRequestException("You need to add some products to the order",400);
            }


            _rabbitManager.Publish("order.create", order);
            return new BaseResponse<string>(default,
                "E-mail with order details will be sent", true);
        }
    }
}
