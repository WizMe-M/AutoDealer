namespace AutoDealer.Web.Utils;

public abstract class MvcController : Controller
{
    protected readonly HttpClient ApiClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    protected MvcController(HttpClient client, IOptions<JsonSerializerOptions> options)
    {
        ApiClient = client;
        _jsonSerializerOptions = options.Value;
    }

    /// <summary>
    /// Receives some data from API by url
    /// </summary>
    /// <param name="uri">URI for HTTP GET Request</param>
    /// <typeparam name="TData">Type that can be received from API</typeparam>
    /// <returns>An API result with data and <see cref="HttpStatusCode"/></returns>
    /// <remarks>Also assigns ModelError 'resp-error' with details of failed request</remarks>
    protected async Task<ApiResult<TData>> GetFromApiAsync<TData>([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
        where TData : class
    {
        var response = await ApiClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<MessageResult>(_jsonSerializerOptions);
            var element = (JsonElement)result!.Data!;
            var data = element.Deserialize<TData>(_jsonSerializerOptions);
            return new ApiResult<TData>(data, HttpStatusCode.OK);
        }

        if (response.StatusCode is HttpStatusCode.Unauthorized)
            throw new ApiNotAuthorizedException();

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        ModelState.AddModelError("resp-error", problemDetails!.Detail!);
        return new ApiResult<TData>((HttpStatusCode)problemDetails.Status!);
    }
}