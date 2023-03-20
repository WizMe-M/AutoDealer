namespace AutoDealer.Web.Utils;

public class ApiResult<T>
{
    public ApiResult(T? value, HttpStatusCode code)
    {
        Value = value;
        StatusCode = code;
    }

    public ApiResult(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public T? Value { get; }

    public HttpStatusCode StatusCode { get; }
}