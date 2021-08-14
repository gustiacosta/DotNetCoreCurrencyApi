using AutoMapper;
using DotNetCoreCurrencyApi.Core.Domain;
using DotNetCoreCurrencyApi.Core.Models;

namespace DotNetCoreCurrencyApi.Infrastructure.AutoMapper
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<ExchangeTransaction, ExchangeTransactionModel>().ReverseMap();
        }
    }
}
