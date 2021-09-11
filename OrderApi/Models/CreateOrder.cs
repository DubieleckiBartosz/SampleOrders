using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class CreateOrder
    {
        [JsonIgnore]
        public Guid ShopId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public IEnumerable<CreateOrderLine> Line { get; set; }
    }
}
