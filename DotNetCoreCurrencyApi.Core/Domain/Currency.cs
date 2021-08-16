using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCurrencyApi.Core.Domain
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal TransactionLimitPerMonth { get; set; }
        public bool RestEnabled { get; set; }
        public string RateApiEndpoint { get; set; }
        public decimal USDRateBase { get; set; }
    }
}
