using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Wrappers
{
    public class BaseResponseStrings
    {
        public const string OrderNull = "Order must not be null";
        public const string RequestSuccess = "Request is successful";
        public const string ShopNull = "Shop must not be null";
        public const string ShopsNotFound = "List of shops is empty";
        public const string ProductsNotFound = "List of products is empty";
        public const string IncorrectData = "Incorrect data was entered";

        public static string DataNotFound(object data)
        {
            return $"{data} not found";
        }
    }
}
