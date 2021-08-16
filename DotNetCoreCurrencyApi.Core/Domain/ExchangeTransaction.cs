using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCurrencyApi.Core.Domain
{
    public class ExchangeTransaction
    {
        [Key]
        public long TransactionId { get; set; }
        public int UserId { get; set; }
        public DateTime TransactionUtcDate { get; set; }
        public decimal OriginAmount { get; set; }
        public string OriginCurrencyCode { get; set; } = "ARS";
        public string DestinationCurrencyCode { get; set; }
        public decimal PurchasedAmount { get; set; }
    }
}
