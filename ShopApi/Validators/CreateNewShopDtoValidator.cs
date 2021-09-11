using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShopApi.AppDatabase;
using ShopApi.Dto.ShopDTO;
using ShopApi.Interfaces;

namespace ShopApi.Validators
{
    public class CreateNewShopDtoValidator:AbstractValidator<CreateShopDto>
    {
        private readonly IAddressService _addressService;
        public CreateNewShopDtoValidator(IAddressService addressService)
        {
            _addressService = addressService;
            RuleFor(r => r.ShopName).NotEmpty()
                .MinimumLength(2);
            RuleFor(w => w).MustAsync(async (s, cancellation) =>
            {
                return await Exist(s.City, s.Street, s.ZipCode);
            }).WithMessage("There is a shop at this address");
        }

        private async Task<bool> Exist(string city, string street, string zipCode)
        {
            var addresses = await _addressService.GetAddressesAsync();
            if (addresses.Any())
            {
                foreach (var addressDto in addresses)
                {
                    if (addressDto.City == city.ToUpper() &&
                        addressDto.Street == street.ToUpper() &&
                        addressDto.ZipCode == zipCode.ToUpper())
                        return false;
                }
                return true;
            }
            return true;
        }
    }
}
