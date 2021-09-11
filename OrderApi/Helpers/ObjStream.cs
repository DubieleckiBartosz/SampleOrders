using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderApi.Exceptions;

namespace OrderApi.Helpers
{
    public static class ObjStream
    {
        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream is null)
            {
                throw new OrderAppException("Can't read from this stream",400);
            }

            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonText = new JsonTextReader(streamReader))
                {
                    var json = new JsonSerializer();
                    var result = json.Deserialize<T>(jsonText);
                    return result;
                }
            }
        }

        public static void SerializeToJson<T>(this Stream stream,T typeObj)
        {
            if (stream is null)
            {
                throw new OrderAppException(nameof(stream), 400);
            }

            using (var streamWriter = new StreamWriter(stream, new UTF8Encoding(), 1024, true))
            {
                using (var jsonTextWrite=new JsonTextWriter(streamWriter))
                {
                    var json = new JsonSerializer();
                    json.Serialize(jsonTextWrite, typeObj);
                    jsonTextWrite.Flush();
                }
            }
        }
    }
}
