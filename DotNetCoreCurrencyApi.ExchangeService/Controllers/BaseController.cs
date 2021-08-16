using AutoMapper;
using DotNetCoreCurrencyApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace DotNetCoreCurrencyApi.ExchangeService.Controllers
{
    public class BaseController : Controller
    {
        internal IMapper _mapper;
        internal IBusinessLogicService _service;
        internal IConfiguration _configuration;
        internal IHttpClientFactory _httpClientFactory;
    }
}
