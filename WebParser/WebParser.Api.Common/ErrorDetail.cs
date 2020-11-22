using Newtonsoft.Json;

namespace WebParser.Api.Common
{
    /// <summary>
    /// Common error object for API in case of error. Including unhandled errors
    /// </summary>
    public class ErrorDetail
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
