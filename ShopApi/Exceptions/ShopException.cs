using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ShopApi.Exceptions
{
    public class ShopException:Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public ShopException():base()
        {
            
        }

        public ShopException(string msg, HttpStatusCode code):base(msg)
        {
            StatusCode = code;
        }
    }
}
