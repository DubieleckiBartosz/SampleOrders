using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderApi.Exceptions
{
    public class NotFoundException:OrderAppException
    {
        public NotFoundException(string msg, int code) : base(msg, code)
        {
        }
    }
}
