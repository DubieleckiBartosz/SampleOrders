using ShopApi.Dto.Orders;
using ShopApi.Entities;
using ShopApi.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopApi.Models;
using ShopApi.QueryParameters;

namespace ShopApi.Interfaces
{
    public interface IOrderService
    {
        //Task<IReadOnlyList<Order>> GetOrdersAsync();
        Task<File> GetOrdersByDataAsync(CsvQuery query);
        Task<BaseResponse<string>> CreateOrderAsync(Guid shopId,CreateOrderDto createOrder);
        Task<BaseResponse<string>> DeleteOrderAsync(Guid orderId);
        Task<BaseResponse<string>> ConfirmOrderAsync(string orderId);
    }
}
