using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ShopApi.Entities;
using ShopApi.Exceptions;

namespace ShopApi.Helpers
{
    public static class OrderSummaryHelper
    {
        public static decimal GetSummaryPriceOrder(this Order order, Dictionary<Guid, Product> products)
        {
            if (order!=null)
            {
                decimal summary=0;
                foreach (var itemLine in order.Line)
                {
                    if (products.TryGetValue(itemLine.ProductId, out Product product))
                    {
                        summary += product.Price * itemLine.Quantity;
                        continue;
                    }
                    else
                    {
                        throw new ShopException("Something went wrong with the price calculation", HttpStatusCode.BadRequest);
                    }
                }

                return Math.Round(summary, 2);
            }
            return default;
        }
    }
}
