using System;

namespace DotNetCoreCurrencyApi.Core.Models
{
    public class ExchangeTransactionModel
    {
        public long TransactionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }        
    }
}
