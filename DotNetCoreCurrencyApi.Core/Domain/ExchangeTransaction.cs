using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCurrencyApi.Core.Domain
{
    public class ExchangeTransaction
    {
        [Key]
        public long TransactionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
