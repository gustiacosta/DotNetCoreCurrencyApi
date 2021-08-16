using DotNetCoreCurrencyApi.Core.Models;
using DotNetCoreCurrencyApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
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
            var server = new TestServer(
                new WebHostBuilder().ConfigureServices(services =>
                {
                    services.AddDbContext<AppDatabaseContext>(options => options.UseInMemoryDatabase("Test"));

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<AppDatabaseContext>();
                        var logger = scopedServices.GetRequiredService<ILogger<ExchangeServiceTest>>();
                        try
                        {
                            db.Database.EnsureCreated();
                            db.Currencies.Add(new Core.Domain.Currency
                            {
                                Id = 1,
                                Code = "USD",
                                RestEnabled = true,
                                RateApiEndpoint = "http://www.bancoprovincia.com.ar/Principal/Dolar",
                                TransactionLimitPerMonth = 200,
                                USDRateBase = 1
                            });
                            db.Currencies.Add(new Core.Domain.Currency
                            {
                                Id = 2,
                                Code = "BRL",
                                RestEnabled = true,
                                RateApiEndpoint = "http://www.bancoprovincia.com.ar/Principal/Dolar",
                                TransactionLimitPerMonth = 300,
                                USDRateBase = 0.25M
                            });
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {Message}}", ex.Message);
                        }
                    }
                })
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
                UserId = 12,
                Amount = 150,
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
