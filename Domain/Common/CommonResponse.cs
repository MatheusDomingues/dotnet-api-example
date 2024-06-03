using api.Domain.Interfaces;
using System.Net;

namespace api.Domain.Common;

public class CommonResponse<T> : ICommonServiceResponse where T : class
{
    public CommonResponse(T? data, bool success, string message, HttpStatusCode statusCode)
    {
        Data = data;
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }

    public T? Data { get; private set; }

    public bool Success { get; private set; }

    public string Message { get; private set; } = string.Empty;

    public HttpStatusCode StatusCode { get; private set; }
}