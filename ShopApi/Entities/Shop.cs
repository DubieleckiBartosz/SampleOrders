using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Entities
{
    public class Shop: BaseEntity
    {
        public Guid Id { get; set; }
        public string ShopName { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
