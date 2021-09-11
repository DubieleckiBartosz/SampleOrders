using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Settings
{
    public class RabbitSettings
    {
        public bool RabbitEnabled { get; set; }
        public string RabbitMqUserName { get; set; }
        public string RabbitMqPassword { get; set; }
        public string RabbitMqHost { get; set; }
        public int RabbitMqPort { get; set; }
        public string VHost { get; set; }
    }
}
