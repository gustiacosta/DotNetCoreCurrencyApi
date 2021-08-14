using AutoMapper;
using DotNetCoreCurrencyApi.Core.Domain;
using DotNetCoreCurrencyApi.Core.Models;
using DotNetCoreCurrencyApi.Services;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static DotNetCoreCurrencyApi.Infrastructure.Validators.ModelValidators;

namespace DotNetCoreCurrencyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyServicesController : BaseController
    {
        private readonly ILogger<CurrencyServicesController> _logger;

        public CurrencyServicesController(ILogger<CurrencyServicesController> logger, IMapper mapper,
                                          IConfiguration _configuration, IHttpClientFactory clientFactory, IBusinessLogicService service)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
            _httpClientFactory = clientFactory;
        }

        [HttpGet]
        [Route("currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            try
            {
                var currencies = await _service.GetAsync<Currency>();

                return Ok(new ResponseModel
                {
                    Message = currencies == null ? "No currencies found" : "Currency list",
                    StatusCode = currencies == null ? StatusCodes.Status404NotFound : StatusCodes.Status200OK,
                    Data = currencies == null ? string.Empty : _mapper.Map<IEnumerable<CurrencyModel>>(currencies)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("currencies/rates/{currencycode}")]
        public async Task<IActionResult> GetRate(string currencycode)
        {
            try
            {
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
                else if (!currency.RateQueryEnabled)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "Errors ocurred",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = "Currency code provided is not allowed for rate query",
                    });
                }

                // call rate api
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("currencies/exchange")]
        public async Task<IActionResult> PurchaseCurrency([CustomizeValidator(Skip = true)] ExchangeTransactionModel model)
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
                    response.Data = "";
                    response.Errors = errors;

                    return BadRequest(response);
                }

                // -------------------------------------------
                // validate user transactions per month limit
                // -------------------------------------------

                return Ok("model ok");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
    }
}
