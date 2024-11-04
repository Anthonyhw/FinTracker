using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTracker.Core.Responses
{
    public class Response<T>
    {
        public readonly int Code;

        [JsonConstructor]
        public Response() => Code = Configuration.DefaultStatusCode;
        
        public Response(T? data, int code = Configuration.DefaultStatusCode, string? message = null)
        {
            Data = data;
            this.Code = code;
            Message = message;
        }
        public T? Data { get; set; }
        public string? Message { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsSuccess => Code is >= 200 and <= 299;
    }
}
