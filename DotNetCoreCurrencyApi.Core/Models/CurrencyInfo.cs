using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreCurrencyApi.Core.Models
{
    public class CurrencyInfo
    {
        public string Code { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
    }
}
