using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Helpers
{
    public static class ObjectOperations
    {
        public static byte[] Serialize<T>(this T obj)
        {
            if (obj is null)
            {
                return null;
            }

            var json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(json);
        }
        public static T Deserialize<T>(this byte[] arrayBytes)
        {
            var json = Encoding.UTF8.GetString(arrayBytes);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
