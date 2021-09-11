using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Enums;

namespace ShopApi.Entities
{
    public class Order: BaseEntity
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public bool IsConfirmed { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public decimal SummaryPrice { get; set; }
        public DateTime? DateOfOrderExecution { get; set; }
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }
        public virtual ICollection<OrderLine> Line { get; set; }

        public Order()
        {
            Status = OrderStatus.Accepted.ToString();
        }
    }
}
