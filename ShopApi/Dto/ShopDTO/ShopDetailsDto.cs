using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApi.Dto.Products;

namespace ShopApi.Dto.ShopDTO
{
    public class ShopDetailsDto
    {
        public string ShopName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
