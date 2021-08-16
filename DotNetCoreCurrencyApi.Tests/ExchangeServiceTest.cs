using DotNetCoreCurrencyApi.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetCoreCurrencyApi.Tests
{
    public class ExchangeServiceTest
    {
        private readonly HttpClient httpClient;
        public ExchangeServiceTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddConfiguration(hostingContext.Configuration);
                    config.AddJsonFile("appsettings.json");
                })
                .UseEnvironment("Development")
                .UseStartup<ExchangeService.Startup>());

            httpClient = server.CreateClient();
        }

        [Fact]
        public async Task PurchaseSuccess()
        {
            var modelTest = new ExchangeTransactionModel
            {
                UserId = 5,
                Amount = 100,
                CurrencyCode = "USD"
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(modelTest), Encoding.UTF8, "application/json");
            using var httpResponse = await httpClient.PostAsync("https://localhost:44369/exchanges/", httpContent);
            var content = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        [Fact]
        public async Task PurchaseErrorExpected()
        {
            var modelTest = new ExchangeTransactionModel
            {
                UserId = 5,
                Amount = 250,
                CurrencyCode = "USD"
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(modelTest), Encoding.UTF8, "application/json");
            using var httpResponse = await httpClient.PostAsync("https://localhost:44369/exchanges/", httpContent);
            var content = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }
    }
}
