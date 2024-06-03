

using System.Net;
using System.Text.Json.Serialization;

namespace api.Domain.Interfaces;

public interface ICommonServiceResponse
{
    public bool Success { get; }
    public string Message { get; }
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; }
}