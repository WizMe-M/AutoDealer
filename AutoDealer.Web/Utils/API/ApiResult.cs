namespace AutoDealer.Web.Utils.API;

public class ApiResult<T>
{
    public ApiResult(T? value, HttpStatusCode code)
    {
        Value = value;
        StatusCode = code;
    }

    public ApiResult(HttpStatusCode statusCode, string details)
    {
        StatusCode = statusCode;
        Details = details;
    }

    public T? Value { get; }

    public HttpStatusCode StatusCode { get; }
    
    public string? Details { get; }
}