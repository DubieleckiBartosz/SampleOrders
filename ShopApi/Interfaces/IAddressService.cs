using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApi.Dto.AddressDTO;
using ShopApi.Entities;

namespace ShopApi.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetAddressesAsync();
    }
}
