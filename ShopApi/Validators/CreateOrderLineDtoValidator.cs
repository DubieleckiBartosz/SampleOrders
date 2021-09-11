using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ShopApi.Dto.OrderLines;

namespace ShopApi.Validators
{
    public class CreateOrderLineDtoValidator:AbstractValidator<CreateOrderLineDto>
    {
        public CreateOrderLineDtoValidator()
        {
            RuleFor(w => w).NotEmpty();
            RuleFor(r => r.Quantity)
                .GreaterThan(0);
            RuleFor(r => r.ProductId).NotEmpty();
        }
    }
}
