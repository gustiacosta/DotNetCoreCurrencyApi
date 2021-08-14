namespace DotNetCoreCurrencyApi.Core.Models
{
    public class CurrencyModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal TransactionLimitPerMonth { get; set; }
        public bool RateQueryEnabled { get; set; }
        public string RateApiEndpoint { get; set; }
        public decimal USDRateBase { get; set; }
    }
}
