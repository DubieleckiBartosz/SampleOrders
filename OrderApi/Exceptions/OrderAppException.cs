using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderApi.Exceptions
{
    public class OrderAppException:Exception
    {
        public int StatusCode { get; private set; }
        public string Message { get; set; }

        public OrderAppException() : base()
        {
                
        }
        public OrderAppException(string msg, int code):base(msg)
        {
            StatusCode = code;
        }
    }
}
