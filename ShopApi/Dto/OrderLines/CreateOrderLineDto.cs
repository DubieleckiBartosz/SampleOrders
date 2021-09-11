using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Dto.OrderLines
{
    public class CreateOrderLineDto
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
