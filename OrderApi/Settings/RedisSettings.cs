using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Settings
{
    public class RedisSettings
    {
        public bool Enabled { get; set; }
        public int TimeInMinutes { get; set; }
        public string Connection { get; set; }
    }
}
