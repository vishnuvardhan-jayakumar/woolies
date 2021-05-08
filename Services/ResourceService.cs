using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Woolies.Model;
using Woolies.Model.Exception;

namespace Woolies.Services
{
    public class ResourceService : IResourceService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<WooliesXConfig> _wooliesXConfig;

        public ResourceService(HttpClient httpClient, IOptions<WooliesXConfig> wooliesXConfig)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(wooliesXConfig.Value.ResourceBaseUrl);
            _wooliesXConfig = wooliesXConfig;
        }

        public async Task<IEnumerable<Order>> GetShoppingHistory()
        {
            var stringResponse = await GetResponse("shopperHistory");
            return JsonConvert.DeserializeObject<IEnumerable<Order>>(stringResponse);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var stringResponse = await GetResponse("products");
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(stringResponse);
        }

        public async Task<decimal> GetTrolleyTotal(TrolleyCalculatorRequest request)
        {
            var token = _wooliesXConfig.Value.User.Token;

            var responseMessage  = await _httpClient.PostAsJsonAsync($"trolleyCalculator?token={token}", request);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new DownstreamFailureException(
                    $"failed with status code {responseMessage.StatusCode}");
            }
            
            return decimal.Parse(stringResponse);
        }

        private async Task<string> GetResponse(string uri)
        {
            var token = _wooliesXConfig.Value.User.Token;
            
            var responseMessage = await _httpClient.GetAsync($"{uri}?token={token}");

            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new DownstreamFailureException(
                    $"failed with status code {responseMessage.StatusCode}");
            }
            
            return stringResponse;
        }
    }
}