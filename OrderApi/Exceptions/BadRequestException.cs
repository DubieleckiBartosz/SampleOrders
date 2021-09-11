using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Exceptions
{
    public class BadRequestException: OrderAppException
    {
        public BadRequestException(string msg,int code):base(msg,code)
        {
                
        }
    }
}
