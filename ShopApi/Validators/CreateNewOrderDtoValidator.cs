using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ShopApi.Dto.Orders;

namespace ShopApi.Validators
{
    public class CreateNewOrderDtoValidator:AbstractValidator<CreateOrderDto>
    {
        public CreateNewOrderDtoValidator()
        {
            RuleFor(r => r.Address).NotEmpty();
            RuleFor(r => r.Email).EmailAddress();
        }
    }
}
