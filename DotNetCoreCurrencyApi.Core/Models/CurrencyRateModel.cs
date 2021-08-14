namespace DotNetCoreCurrencyApi.Core.Models
{
    public class CurrencyRateModel
    {
        public string Code { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
    }
}
