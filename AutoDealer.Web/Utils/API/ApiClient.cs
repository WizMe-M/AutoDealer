namespace AutoDealer.Web.Utils.API;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public ApiClient(HttpClient httpClient, IOptions<JsonSerializerOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

#if DEBUG
        _httpClient.BaseAddress = new Uri("https://localhost:7138/");
#else
        _httpClient.BaseAddress = new Uri("https://api:44357/");
#endif
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
    }

    public void SetAuthorization(string token) => _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

    public void ResetAuthorization() => _httpClient.DefaultRequestHeaders.Authorization = null;

    /// <summary>
    /// Get some data from API by uri
    /// </summary>
    /// <param name="uri">URI for HTTP GET Request</param>
    /// <typeparam name="TOut">Type that can be received from API</typeparam>
    /// <returns>An API result with data and <see cref="HttpStatusCode"/></returns>
    /// <remarks>Also assigns ModelError 'resp-error' with details of failed request</remarks>
    public async Task<ApiResult<TOut>> GetAsync<TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
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
    public async Task<ApiResult<TOut>> PostAsync<TIn, TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
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
    public async Task<ApiResult<TOut>> PutAsync<TIn, TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
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
    public async Task<ApiResult<TOut>> PatchAsync<TIn, TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        TIn content) where TOut : class => await ExecuteApiRequest<TIn, TOut>(uri, HttpMethod.Patch, content);


    /// <summary>
    /// Delete some data at API by uri
    /// </summary>
    /// <param name="uri">URI for HTTP GET Request</param>
    /// <typeparam name="TOut">Type that can be received from API</typeparam>
    /// <returns>An API result with data and <see cref="HttpStatusCode"/></returns>
    /// <remarks>Also assigns ModelError 'resp-error' with details of failed request</remarks>
    public async Task<ApiResult<TOut>> DeleteAsync<TOut>([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
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
            HttpMethod.Get => _httpClient.GetAsync(uri),
            HttpMethod.Post => _httpClient.PostAsync(uri, body),
            HttpMethod.Patch => _httpClient.PatchAsync(uri, body),
            HttpMethod.Put => _httpClient.PutAsync(uri, body),
            HttpMethod.Delete => _httpClient.DeleteAsync(uri),
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
        return new ApiResult<TResult>((HttpStatusCode)problemDetails!.Status!, problemDetails.Detail!);
    }

    private async Task<TOut?> Deserialize<TOut>(HttpResponseMessage response) where TOut : class
    {
        var result = await response.Content.ReadFromJsonAsync<MessageResult>(_options);
        var json = result!.Data!.ToString()!;
        var data = JsonSerializer.Deserialize<TOut>(json, _options);
        return data;
    }
}