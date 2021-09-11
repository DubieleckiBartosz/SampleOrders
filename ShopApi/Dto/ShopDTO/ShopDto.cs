using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Dto.ShopDTO
{
    public class ShopDto
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string ShopName { get; set; }
    }
}
