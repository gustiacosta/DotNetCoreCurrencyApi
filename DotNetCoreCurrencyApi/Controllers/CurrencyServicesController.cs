using AutoMapper;
using DotNetCoreCurrencyApi.Core.Domain;
using DotNetCoreCurrencyApi.Core.Models;
using DotNetCoreCurrencyApi.Data.Services;
using DotNetCoreCurrencyApi.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNetCoreCurrencyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyServicesController : BaseController
    {
        private readonly ILogger<CurrencyServicesController> _logger;

        public CurrencyServicesController(ILogger<CurrencyServicesController> logger, IMapper mapper,
                                          IHttpClientFactory clientFactory, IBusinessLogicService service)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
            _httpClientFactory = clientFactory;
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Comments: 
        // The text result from calling the dolar api endpoint is not a clean json data, it doesn't have property names, so we need to parse it in a different way.
        // We can have a json template from each api endpoint saved with each currency in our db, so the parsing method will be dynamic and easier to extend
        // -----------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Route("currencies/rates/{currencycode}")]
        public async Task<IActionResult> GetRate(string currencycode)
        {
            try
            {
                _logger.LogInformation($"Currency code {currencycode} rate requested:");

                if (string.IsNullOrEmpty(currencycode))
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "Errors ocurred",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = "Currency code is empty"
                    });
                }
                else if (currencycode.Length != 3)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "Errors ocurred",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = "Currency code length is not valid"
                    });
                }

                var currency = await _service.GetFirstAsync<Currency>(c => c.Code.ToLower().Equals(currencycode.ToLower()));
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
                        Errors = "Currency code provided is not allowed for rate query",
                    });
                }

                // ----------------------------------------
                // call rate api endpoint
                // ----------------------------------------
                CurrencyRateModel currencyRate = await HttpClientHelper.CallExternalRateEndpoint(currency, _httpClientFactory);
                if (currencyRate == null)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "No rate found",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Data = $"We couldn't get the rate for {currencycode} for today",
                    });
                }

                // ---------------------------------------------------------------------------------
                // here we calculate a rate to get the value for each currency
                // we have a base ratio saved in database to calculate the rates based on USD prices                    
                // ---------------------------------------------------------------------------------
                return Ok(new ResponseModel
                {
                    Message = $"Here is the rate for {currencycode.ToUpper()}",
                    StatusCode = StatusCodes.Status200OK,
                    Data = new CurrencyRateModel
                    {
                        Code = currencycode.ToUpper(),
                        PurchasePrice = currencyRate.PurchasePrice * currency.USDRateBase,
                        SellPrice = currencyRate.SellPrice * currency.USDRateBase
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return BadRequest();
        }

        private JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true
            };
        }
    }
}
