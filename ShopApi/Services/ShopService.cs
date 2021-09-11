using ShopApi.Dto.ShopDTO;
using ShopApi.Interfaces;
using ShopApi.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using ShopApi.AppDatabase;
using ShopApi.Entities;
using ILogger = Serilog.ILogger;

namespace ShopApi.Services
{
    public class ShopService :BaseContextService<Shop>, IShopService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ShopService> _logger;
        public ShopService(ApplicationDbContext db,IMapper mapper,ILogger<ShopService> logger):base(db)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IReadOnlyList<ShopDto>>> GetAllShopsAsync()
        {
            var result = await _db.Shops.Include(e=>e.Address).ToListAsync();
                if (result.Any())
                {
                    var resultMap = _mapper.Map<IEnumerable<ShopDto>>(result);
                    return BaseResponse<IReadOnlyList<ShopDto>>.Ok(resultMap.ToList());
                }
                return BaseResponse<IReadOnlyList<ShopDto>>.Error(BaseResponseStrings.ShopsNotFound);
        }

        public async Task<BaseResponse<ShopDto>> CreateShopAsync(CreateShopDto shopDto)
        {
            if (shopDto is null)
            {
                return BaseResponse<ShopDto>.Error(BaseResponseStrings.ShopNull);
            }

            var shop = _mapper.Map<Shop>(shopDto);
            var entity = await _db.Shops.AddAsync(shop);
            await _db.SaveChangesAsync();
            _logger.LogInformation($"Created new shop {entity.Entity.Id}");
            var shopMap = _mapper.Map<ShopDto>(entity.Entity);
            return BaseResponse<ShopDto>.Ok(shopMap);
        }

        public async Task<BaseResponse<bool>> DeleteShopAsync(Guid shopId)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.Id == shopId);
            if (shop is not null)
            { 
                _db.Shops.Remove(shop);
               await _db.SaveChangesAsync();
               _logger.LogInformation($"Shop {shopId} deleted");
               return BaseResponse<bool>.Ok(true);
            }
            return BaseResponse<bool>.Error(BaseResponseStrings.DataNotFound(shopId));
        }


        public async Task<BaseResponse<ShopDto>> UpdateShopAsync(Guid shopId,UpdateShopDto shopDto)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.Id == shopId);
            if (shop is null)
            {
                return BaseResponse<ShopDto>.Error(BaseResponseStrings.DataNotFound(shopId));
            }
            _mapper.Map(shopDto, shop);
            var result=_db.Shops.Update(shop);
            await _db.SaveChangesAsync();
            return BaseResponse<ShopDto>.Ok(_mapper.Map<ShopDto>(result.Entity));

        }

        public async Task<BaseResponse<ShopDetailsDto>> GetDetailsShopAsync(Guid shopId)
        {
            (bool exist, Shop shop) =await ShopExistAsync(shopId);
            if (exist)
            {
                var result = await _db.Shops.Include(s => s.Address)
                    .Include(s => s.Products).FirstOrDefaultAsync();
                return BaseResponse<ShopDetailsDto>.Ok(_mapper.Map<ShopDetailsDto>(result));
            }
            return BaseResponse<ShopDetailsDto>.Error(BaseResponseStrings.ShopNull);
        }

        public async Task<(bool, Shop)> ShopExistAsync(Guid id)
        {
            var shop=await _db.Shops.FirstOrDefaultAsync(s => s.Id == id);
                return shop is null ? (false, null)
                : (true, shop);
        }
          



    }
}
