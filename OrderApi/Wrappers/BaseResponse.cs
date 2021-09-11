using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Wrappers
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public BaseResponse() { }

        public BaseResponse(T data)
        {
            (Data, Success) = (data, true);
        }
        public BaseResponse(T data, string message, bool success)
        {
            (Data, Message, Success) = (data, message, success);
        }
        public BaseResponse(T data, string message,
            bool success, IEnumerable<string> errors) : this(data, message, success)
        {
            Errors = errors;
        }
    }
}
