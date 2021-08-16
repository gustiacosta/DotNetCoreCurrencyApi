using AutoMapper;
using AutoMapper.Configuration;
using DotNetCoreCurrencyApi.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace DotNetCoreCurrencyApi.Controllers
{
    public class BaseController : Controller
    {
        internal IMapper _mapper;
        internal IBusinessLogicService _service;
        internal IConfiguration _configuration;
        internal IHttpClientFactory _httpClientFactory;
    }
}
