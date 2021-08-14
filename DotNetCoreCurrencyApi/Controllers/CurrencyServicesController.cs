using DotNetCoreCurrencyApi.Core.Domain;
using DotNetCoreCurrencyApi.Core.Models;
using DotNetCoreCurrencyApi.Services;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static DotNetCoreCurrencyApi.Infrastructure.Validators.ModelValidators;

namespace DotNetCoreCurrencyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyServicesController : ControllerBase
    {
        private readonly IBusinessLogicService _service;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CurrencyServicesController> _logger;

        public CurrencyServicesController(ILogger<CurrencyServicesController> logger,
                                          IHttpClientFactory clientFactory, IBusinessLogicService service)
        {
            _logger = logger;
            _service = service;
            _httpClientFactory = clientFactory;
        }

        [HttpGet]
        [Route("exchange/rates/{currencycode}")]
        public async Task<IActionResult> GetRate(string currencycode)
        {
            try
            {
                // ----------------------------------------
                // validate queried currency code enabled
                // ----------------------------------------
                var isCurrencyCodeAllowed = _service.GetFirstAsync<Currency>(c => c.Code.Equals(currencycode, StringComparison.OrdinalIgnoreCase) && c.RateQueryEnabled) != null;
                if (!isCurrencyCodeAllowed)
                {
                    return BadRequest(new ResponseModel
                    {
                        Message = "Currency code provided is not allowed for rate query",
                        StatusCode = StatusCodes.Status404NotFound,
                        Data = ""
                    });
                }

                // call rate api

                //response.Message = "";
                //response.StatusCode = StatusCodes.Status200OK;
                //response.Data = "";

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("exchange/transactions")]
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
