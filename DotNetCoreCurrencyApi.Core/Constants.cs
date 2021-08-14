using System.Collections.Generic;

namespace DotNetCoreCurrencyApi.Core
{
    public class Constants
    {
        public static string HttpClientFactoryName = "MyHttpClient";
        public static List<string> AllowedCurrencies = new() { "USD", "BRL" };
    }
}
