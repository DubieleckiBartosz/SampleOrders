using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.Wrappers;

namespace OrderApi.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<string>> CreateOrderAsync(CreateOrder order);
    }
}
