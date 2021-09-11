using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ShopApi.QueryParameters;

namespace ShopApi.Validators
{
    public class CsvQueryValidator:AbstractValidator<CsvQuery>
    {
        public CsvQueryValidator()
        {
            RuleFor(r => r).Must(s =>
            {
                if ((s.MaxSumPrice != null) && (s.MinSumPrice != null))
                {
                    return (s.MaxSumPrice < s.MinSumPrice) ? false : true;
                }
                return true;
            });
            RuleFor(r => r.MaxSumPrice).GreaterThan(5);
            RuleFor(r => r.MinSumPrice).GreaterThanOrEqualTo(0);
        }
    }
}
