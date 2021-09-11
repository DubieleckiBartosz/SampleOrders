using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApi.Dto.ShopDTO;
using ShopApi.Entities;
using ShopApi.Wrappers;

namespace ShopApi.Interfaces
{
    public interface IShopService
    {
        Task<BaseResponse<ShopDetailsDto>> GetDetailsShopAsync(Guid shopId);
        Task<BaseResponse<IReadOnlyList<ShopDto>>> GetAllShopsAsync();
        Task<BaseResponse<ShopDto>> CreateShopAsync(CreateShopDto shopDto);
        Task<BaseResponse<bool>> DeleteShopAsync(Guid shopId);
        Task<BaseResponse<ShopDto>> UpdateShopAsync(Guid shopId,UpdateShopDto shopDto);
        Task<(bool, Shop)> ShopExistAsync(Guid id);
    }
}
