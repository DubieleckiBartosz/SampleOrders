using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderApi.Exceptions;
using OrderApi.Helpers;
using OrderApi.Interfaces;
using OrderApi.Models;
using OrderApi.Wrappers;

namespace OrderApi.Services
{
    public class ShopClientService:IShopClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShopClientService> _logger;
        public ShopClientService(HttpClient httpClient,ILogger<ShopClientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<BaseResponse<ShopDetails>> GetShopDetailsAsync(Guid shopId, 
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/shop/{shopId}");
            try
            {
                using var result = await _httpClient.SendAsync(request,
                     HttpCompletionOption.ResponseHeadersRead,cancellationToken);
                if (!result.IsSuccessStatusCode)
                {
                    var error =await result.Content.ReadAsStringAsync();
                    var resultError= JsonConvert.DeserializeObject<Error>(error);
                    throw new ApiClientException(resultError.Message, (int) result.StatusCode);
                }
                result.EnsureSuccessStatusCode();
                var shopDetails=await result.ResponseClient<BaseResponse<ShopDetails>>();
                return shopDetails;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<BaseResponse<IEnumerable<Shop>>> GetShopsAsync(
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/shop");
            try
            {
                using var result = await _httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);

                if (!result.IsSuccessStatusCode)
                {
                    var error = await result.Content.ReadAsStringAsync();
                    var resultError = JsonConvert.DeserializeObject<Error>(error);
                    throw new ApiClientException(resultError.Message, (int)result.StatusCode);
                }
                result.EnsureSuccessStatusCode();
                return await result.ResponseClient<BaseResponse<IEnumerable<Shop>>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }
    }
}
