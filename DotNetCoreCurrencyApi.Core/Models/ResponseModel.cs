using System.Text.Json.Serialization;

namespace DotNetCoreCurrencyApi.Core.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public dynamic Errors { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public dynamic Data { get; set; }
    }
}
