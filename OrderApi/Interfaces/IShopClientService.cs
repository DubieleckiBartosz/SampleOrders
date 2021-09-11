﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderApi.Models;
using OrderApi.Wrappers;

namespace OrderApi.Interfaces
{
    public interface IShopClientService
    {
        Task<BaseResponse<IEnumerable<Shop>>> GetShopsAsync(CancellationToken cancellationToken = default);
        Task<BaseResponse<ShopDetails>> GetShopDetailsAsync(Guid shopId, CancellationToken cancellationToken = default);
    }
}
