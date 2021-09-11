using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Exceptions
{
    public class ApiClientException:OrderAppException
    {
        public ApiClientException(string msg, int code) : base(msg, code)
        {

        }
    }
}
