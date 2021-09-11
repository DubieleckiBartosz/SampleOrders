using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrderApi.Helpers
{
    public static class ResponseHttpClientHelper
    {

        public static async Task<T> ResponseClient<T>(this HttpResponseMessage msg)
        {
            var stream = await msg.Content.ReadAsStreamAsync();
            return stream.ReadAndDeserializeFromJson<T>();
        }
    }
}
