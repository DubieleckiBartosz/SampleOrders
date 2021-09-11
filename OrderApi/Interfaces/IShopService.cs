using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderApi.Models;
using OrderApi.Wrappers;

namespace OrderApi.Interfaces
{
    public interface IShopService
    {
        Task<BaseResponse<IEnumerable<Shop>>> GetShopsAsync();
        Task<BaseResponse<ShopDetails>> GetShopDetailsAsync(Guid shopId);
    }
}
