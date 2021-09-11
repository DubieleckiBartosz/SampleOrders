using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class CreateOrderLine
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
