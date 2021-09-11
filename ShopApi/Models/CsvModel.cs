using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Models
{
    public class CsvModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string IsConfirmed { get; set; }
        public string Email { get; set; }
        public decimal SummaryPrice { get; set; }
        public string CreatedDate { get; set; }
        public string DateOfOrderExecution { get; set; }
    }
}
