using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DotNetCoreCurrencyApi.Tests
{
    public class CurrencyServiceTest
    {
        private readonly HttpClient httpClient;
        public CurrencyServiceTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddConfiguration(hostingContext.Configuration);
                    config.AddJsonFile("appsettings.json");
                })
                .UseEnvironment("Development")
                .UseStartup<Startup>());

            httpClient = server.CreateClient();
        }

        [Fact]
        public async Task ApiCallSuccess()
        {
            using var httpResponse = await httpClient.GetAsync("https://localhost:44366/CurrencyServices/currencies/rates/USD");
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        [Fact]
        public async Task ApiCallErrorExpected()
        {
            using var httpResponse = await httpClient.GetAsync("https://localhost:44366/CurrencyServices/currencies/rates/BRLL");

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Theory]
        [InlineData(new object[] { "ARS", "PYG" })]
        public async Task ApiCallCurrencyNotFoundExpected(params string[] codes)
        {
            foreach (var code in codes)
            {
                using var httpResponse = await httpClient.GetAsync($"https://localhost:44366/CurrencyServices/currencies/rates/" + code);
                Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            }
        }
    }
}
