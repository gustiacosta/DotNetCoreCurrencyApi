using AutoMapper;
using DotNetCoreCurrencyApi.Core.Domain;
using DotNetCoreCurrencyApi.Core.Models;

namespace DotNetCoreCurrencyApi.Infrastructure.AutoMapper
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Currency, CurrencyModel>().ReverseMap();
            CreateMap<ExchangeTransaction, ExchangeTransactionModel>().ReverseMap();
        }
    }
}
