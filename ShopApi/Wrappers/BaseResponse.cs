using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Wrappers
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Success{ get; set; }
        public IEnumerable<string> Errors { get; set; }

        public BaseResponse(){}

        public BaseResponse(T data,string message,bool success)
        {
            (Data, Message, Success) = (data, message, success);
        }
        public BaseResponse(T data, string message,
            bool success,IEnumerable<string> errors):this(data,message,success)
        {
            Errors = errors;
        }
        public static BaseResponse<T> Ok(T data)
        {
            return new BaseResponse<T>(data, null, true);
        }
        public static BaseResponse<T> Ok(T data,string msg)
        {
            return new BaseResponse<T>(data, msg, true);
        }
        public static BaseResponse<T> Error(string msg)
        {
            return new BaseResponse<T>(default(T), msg, false);
        }
        public static BaseResponse<T> Error(string msg,IEnumerable<string> errors)
        {
            return new BaseResponse<T>(default(T), msg, false,errors);
        }
    }
}
