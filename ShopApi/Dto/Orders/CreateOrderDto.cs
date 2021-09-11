using ShopApi.Dto.OrderLines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopApi.Dto.Orders
{
    public class CreateOrderDto
    {
        [JsonIgnore]
        public Guid ShopId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        [Phone]
        [Required]
        public string NumberPhone { get; set; }
        public IEnumerable<CreateOrderLineDto> Line { get; set; }
    }
}
