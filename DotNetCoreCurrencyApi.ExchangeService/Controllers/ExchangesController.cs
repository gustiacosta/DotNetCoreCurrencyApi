using AutoMapper;
using DotNetCoreCurrencyApi.Core.Domain;
using DotNetCoreCurrencyApi.Core.Models;
using DotNetCoreCurrencyApi.Data.Services;
using DotNetCoreCurrencyApi.Infrastructure.Helpers;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static DotNetCoreCurrencyApi.Infrastructure.Validators.ModelValidators;

namespace DotNetCoreCurrencyApi.ExchangeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangesController : BaseController
    {
        private readonly ILogger<ExchangesController> _logger;

        public ExchangesController(ILogger<ExchangesController> logger, IMapper mapper,
                                   IHttpClientFactory clientFactory, IBusinessLogicService service, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
            _configuration = configuration;
            _httpClientFactory = clientFactory;
        }

        /// <summary>
        /// This calls our rates microservice to get the current rates and make a currency purchase
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Purchase([CustomizeValidator(Skip = true)] ExchangeTransactionModel model)
        {
            try
            {
                var response = new ResponseModel();
                ExchangeTransactionModelValidator validator = new ExchangeTransactionModelValidator();
                ValidationResult validationResults = await validator.ValidateAsync(model);

                // ----------------------------------------
                // validate input
                // ----------------------------------------
                if (!validationResults.IsValid)
                {
                    var errors = new Dictionary<string, string>();
                    foreach (var error in validationResults.Errors)
                    {
                        errors.Add(error.PropertyName, error.ErrorMessage);
                    }

                    response.Message = "Some errors ocurred";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Errors = errors;

                    return BadRequest(response);
                }

                // ----------------------------------------
                // log input data received
                // ----------------------------------------
                var serializedData = JsonConvert.SerializeObject(model);
                _logger.LogInformation($"Purchase data: {serializedData}");

                var currency = await _service.GetFirstAsync<Currency>(c => c.Code.ToLower().Equals(model.CurrencyCode.ToLower()));
                if (currency == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "Errors ocurred",
                        StatusCode = StatusCodes.Status404NotFound,
                        Errors = "Currency not found"
                    });
                }
                else if (!currency.RestEnabled)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "Errors ocurred",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = "Currency code provided is not allowed for rate/purchase",
                    });
                }
                else if (model.Amount > currency.TransactionLimitPerMonth)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "Errors ocurred",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = $"Entered mount is out of limits [{currency.TransactionLimitPerMonth}]",
                    });
                }

                // set transaction date
                var transactionDate = DateTime.UtcNow;

                // --------------------------------------------------------
                // validate user transaction limits per month and currency
                // --------------------------------------------------------
                var userTransactions = await _service.GetAsync<ExchangeTransaction>(x => x.UserId == model.UserId &&
                                                               x.TransactionUtcDate.Month == transactionDate.Month &&
                                                               x.DestinationCurrencyCode.ToLower().Equals(model.CurrencyCode.ToLower()));
                if (userTransactions.Any())
                {
                    if ((userTransactions.Select(c => c.PurchasedAmount).Sum() + model.Amount) > currency.TransactionLimitPerMonth)
                    {
                        return BadRequest(new ResponseModel
                        {
                            Message = "Errors ocurred",
                            StatusCode = StatusCodes.Status400BadRequest,
                            Errors = $"Entered mount {currency.TransactionLimitPerMonth} is out of limits for this user {model.UserId} and month",
                        });
                    }
                }

                // ----------------------------------------------------------------
                // here we're calling our rates microservice
                // ----------------------------------------------------------------
                string apiUrl = _configuration["ApiEndpoints:RatesEndpoint"].Replace("{currencyCode}", model.CurrencyCode);
                var ratesResponse = await HttpClientHelper.CallRatesMicroservice(currency, _httpClientFactory, apiUrl);
                if (ratesResponse == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "No rate found",
                        StatusCode = StatusCodes.Status404NotFound,
                        Data = $"We couldn't get the rate for {model.CurrencyCode} for today",
                    });
                }

                CurrencyRateModel currencyRate = JsonConvert.DeserializeObject<CurrencyRateModel>(ratesResponse.Data.ToString());
                if (currencyRate == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "No rate found",
                        StatusCode = StatusCodes.Status404NotFound,
                        Data = $"We couldn't get the rate for {model.CurrencyCode} for today",
                    });
                }

                // ----------------------------------------------------------------
                // using sell price for the purchase operation
                // documentation says to calculate the value in this way
                // ----------------------------------------------------------------
                decimal purchasedAmount = Math.Round(model.Amount / currencyRate.SellPrice, 5);

                var transaction = new ExchangeTransaction
                {
                    UserId = model.UserId,
                    OriginAmount = model.Amount,
                    DestinationCurrencyCode = model.CurrencyCode.ToUpper(),
                    TransactionUtcDate = transactionDate,
                    PurchasedAmount = purchasedAmount
                };

                _service.Create<ExchangeTransaction>(transaction);
                await _service.SaveAsync();

                return Ok(new ResponseModel
                {
                    Message = "Transaction succesfull",
                    StatusCode = StatusCodes.Status200OK,
                    Data = $"Transaction Id: {transaction.TransactionId}; Purchased amount: {purchasedAmount:N5} {transaction.DestinationCurrencyCode}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

    }
}
