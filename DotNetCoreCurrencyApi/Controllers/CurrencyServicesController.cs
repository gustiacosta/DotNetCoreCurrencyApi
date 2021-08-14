using DotNetCoreCurrencyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetCoreCurrencyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyServicesController : ControllerBase
    {
        private readonly IBusinessLogicService _service;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<CurrencyServicesController> _logger;

        public CurrencyServicesController(ILogger<CurrencyServicesController> logger,
                                          IHttpClientFactory clientFactory, IBusinessLogicService service)
        {
            _logger = logger;
            _service = service;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        [Route("currency/get-current")]
        public async Task<IActionResult> GetCurrent()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
