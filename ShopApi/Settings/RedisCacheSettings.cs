using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Settings
{
    public class RedisCacheSettings
    {
        public bool Enabled { get; set; }
        public string Connection { get; set; }
    }
}
