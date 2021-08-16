using DotNetCoreCurrencyApi.Core.Models;
using Newtonsoft.Json.Linq;
using System;

namespace DotNetCoreCurrencyApi.Infrastructure.Helpers
{
    public class CurrencyHelper
    {
        /// <summary>
        /// The parsing method will try to parse json string values for each country, as maybe will be different api endpoints
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="currencyCode"></param>
        /// <param name="jsonTemplate"></param>
        /// <returns></returns>
        public static CurrencyRateModel ParseData(string raw, string currencyCode, string jsonTemplate)
        {
            decimal sellPrice = 0;
            decimal purchasePrice = 0;

            try
            {
                switch (currencyCode)
                {
                    case "USD":
                        dynamic json = JValue.Parse(raw);
                        if (json != null)
                        {
                            decimal.TryParse(json[0].ToString(), out purchasePrice);     //assuming first value as purchase price
                            decimal.TryParse(json[1].ToString(), out sellPrice);         //assuming second value as purchase price
                        }
                        break;

                    case "BRL":
                        // ------------------------------------------------
                        // Simulating a different api endpoint call
                        // In this case, the endpoint is the same for USD
                        // but preparing for scaling in future
                        // ------------------------------------------------
                        json = JValue.Parse(raw);
                        if (json != null)
                        {
                            decimal.TryParse(json[0].ToString(), out purchasePrice);     //assuming first value as purchase price
                            decimal.TryParse(json[1].ToString(), out sellPrice);         //assuming second value as purchase price
                        }
                        break;
                }

                // ----------------------------------------
                // we'll be returning the same rate model 
                // ----------------------------------------
                return new CurrencyRateModel
                {
                    Code = currencyCode,
                    SellPrice = sellPrice,
                    PurchasePrice = purchasePrice
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }                
    }
}
