using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCurrencyApi.Core.Domain
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal TransactionLimitPerMonth { get; set; }
        public bool RateQueryEnabled { get; set; }
    }
}
