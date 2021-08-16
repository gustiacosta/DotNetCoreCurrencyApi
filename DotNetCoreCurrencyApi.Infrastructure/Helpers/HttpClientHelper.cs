using DotNetCoreCurrencyApi.Core.Domain;
using DotNetCoreCurrencyApi.Core.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetCoreCurrencyApi.Infrastructure.Helpers
{
    public class HttpClientHelper
    {

        public static async Task<CurrencyRateModel> CallExternalRateEndpoint(Currency currency, IHttpClientFactory _httpClientFactory)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClientFactoryName);
            using (var httpResponse = await httpClient.GetAsync(currency.RateApiEndpoint))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    return CurrencyHelper.ParseData(content, currency.Code, "");
                }
            }

            return null;
        }

        public static async Task<ResponseModel> CallRatesMicroservice(Currency currency, IHttpClientFactory _httpClientFactory, string endpoint)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClientFactoryName);
            using (var httpResponse = await httpClient.GetAsync(endpoint))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var responseModel = JsonConvert.DeserializeObject<ResponseModel>(content);

                    return responseModel;
                }
            }

            return null;
        }
    }
}