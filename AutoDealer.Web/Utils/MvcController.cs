namespace AutoDealer.Web.Utils;

public abstract class MvcController : Controller
{
    protected readonly HttpClient ApiClient;
    protected readonly JsonSerializerOptions JsonSerializerOptions;

    protected MvcController(HttpClient client, IOptions<JsonSerializerOptions> options)
    {
        ApiClient = client;
        JsonSerializerOptions = options.Value;
    }
}