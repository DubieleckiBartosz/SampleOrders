using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopApi.AppDatabase;
using ShopApi.Dto.AddressDTO;
using ShopApi.Entities;
using ShopApi.Interfaces;

namespace ShopApi.Services
{
    public class AddressService : BaseContextService<Address>, IAddressService
    {
        private readonly IMapper _mapper;
        public AddressService(ApplicationDbContext db,IMapper mapper):base(db)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressDto>> GetAddressesAsync()
        { 
            var addresses = await _db.Addresses.ToListAsync();
            return _mapper.Map<IEnumerable<AddressDto>>(addresses);
        }
           

    }
}
