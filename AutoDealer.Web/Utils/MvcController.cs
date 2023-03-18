using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AutoDealer.Web.Utils;

public abstract class MvcController : Controller
{
    protected readonly HttpClient ApiClient;

    protected MvcController(HttpClient client)
    {
        ApiClient = client;
#if DEBUG
        ApiClient.BaseAddress = new Uri("https://localhost:7138/");
#else
        ApiClient.BaseAddress = new Uri("https://api:44357/");
#endif
        ApiClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
    }
}