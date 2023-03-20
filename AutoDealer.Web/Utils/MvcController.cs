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
    /// Get some data from API by uri
    /// </summary>
    /// <param name="uri">URI for HTTP GET Request</param>
    /// <typeparam name="TOut">Type that can be received from API</typeparam>
    /// <returns>An API result with data and <see cref="HttpStatusCode"/></returns>
    /// <remarks>Also assigns ModelError 'resp-error' with details of failed request</remarks>
    protected async Task<ApiResult<TOut>> GetApiAsync<TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
        where TOut : class => await ExecuteApiRequest<object, TOut>(uri, HttpMethod.Get);

    /// <summary>
    /// Post some data to API by uri
    /// </summary>
    /// <param name="uri">URI for HTTP GET Request</param>
    /// <param name="content">Content that will be sent in HTTP Body</param>
    /// <typeparam name="TIn">Type that will be serialized as JSON into HTTP Body</typeparam>
    /// <typeparam name="TOut">Type that can be received from API</typeparam>
    /// <returns>An API result with data and <see cref="HttpStatusCode"/></returns>
    /// <remarks>Also assigns ModelError 'resp-error' with details of failed request</remarks>
    protected async Task<ApiResult<TOut>> PostApiAsync<TIn, TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        TIn content) where TOut : class => await ExecuteApiRequest<TIn, TOut>(uri, HttpMethod.Post, content);

    /// <summary>
    /// Put some data to API by uri
    /// </summary>
    /// <param name="uri">URI for HTTP GET Request</param>
    /// <param name="content">Content that will be sent in HTTP Body</param>
    /// <typeparam name="TIn">Type that will be serialized as JSON into HTTP Body</typeparam>
    /// <typeparam name="TOut">Type that can be received from API</typeparam>
    /// <returns>An API result with data and <see cref="HttpStatusCode"/></returns>
    /// <remarks>Also assigns ModelError 'resp-error' with details of failed request</remarks>
    protected async Task<ApiResult<TOut>> PutApiAsync<TIn, TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        TIn content) where TOut : class => await ExecuteApiRequest<TIn, TOut>(uri, HttpMethod.Put, content);

    /// <summary>
    /// Patch some data to API by uri
    /// </summary>
    /// <param name="uri">URI for HTTP GET Request</param>
    /// <param name="content">Content that will be sent in HTTP Body</param>
    /// <typeparam name="TIn">Type that will be serialized as JSON into HTTP Body</typeparam>
    /// <typeparam name="TOut">Type that can be received from API</typeparam>
    /// <returns>An API result with data and <see cref="HttpStatusCode"/></returns>
    /// <remarks>Also assigns ModelError 'resp-error' with details of failed request</remarks>
    protected async Task<ApiResult<TOut>> PatchApiAsync<TIn, TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        TIn content) where TOut : class => await ExecuteApiRequest<TIn, TOut>(uri, HttpMethod.Patch, content);


    /// <summary>
    /// Delete some data at API by uri
    /// </summary>
    /// <param name="uri">URI for HTTP GET Request</param>
    /// <typeparam name="TOut">Type that can be received from API</typeparam>
    /// <returns>An API result with data and <see cref="HttpStatusCode"/></returns>
    /// <remarks>Also assigns ModelError 'resp-error' with details of failed request</remarks>
    protected async Task<ApiResult<TOut>> DeleteApiAsync<TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
        where TOut : class => await ExecuteApiRequest<object, TOut>(uri, HttpMethod.Delete);

    private async Task<ApiResult<TOut>> ExecuteApiRequest<TIn, TOut>(string uri, HttpMethod method, TIn? data = default)
        where TOut : class
    {
        var body = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        var apiRequestTask = method switch
        {
            HttpMethod.Get => ApiClient.GetAsync(uri),
            HttpMethod.Post => ApiClient.PostAsync(uri, body),
            HttpMethod.Patch => ApiClient.PatchAsync(uri, body),
            HttpMethod.Put => ApiClient.PutAsync(uri, body),
            HttpMethod.Delete => ApiClient.DeleteAsync(uri),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
        };
        var response = await apiRequestTask;
        ThrowIfNotAuthorized(response.StatusCode);

        return await GetResultFromResponseAsync<TOut>(response);
    }

    private static void ThrowIfNotAuthorized(HttpStatusCode code)
    {
        if (code is HttpStatusCode.Unauthorized)
            throw new ApiNotAuthorizedException();
    }

    private async Task<ApiResult<TResult>> GetResultFromResponseAsync<TResult>(HttpResponseMessage response)
        where TResult : class
    {
        if (response.IsSuccessStatusCode)
        {
            var data = await Deserialize<TResult>(response);
            return new ApiResult<TResult>(data, response.StatusCode);
        }

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        AssignProblemDetails(problemDetails!);
        return new ApiResult<TResult>((HttpStatusCode)problemDetails!.Status!);
    }

    private async Task<TOut?> Deserialize<TOut>(HttpResponseMessage response) where TOut : class
    {
        var result = await response.Content.ReadFromJsonAsync<MessageResult>(_jsonSerializerOptions);
        var element = (JsonElement)result!.Data!;
        return element.Deserialize<TOut>(_jsonSerializerOptions);
    }

    private void AssignProblemDetails(ProblemDetails problemDetails) =>
        ModelState.AddModelError("resp-error", problemDetails.Detail!);
}