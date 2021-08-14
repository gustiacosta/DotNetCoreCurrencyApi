namespace DotNetCoreCurrencyApi.Core.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
        public object Data { get; set; } = string.Empty;
    }
}
