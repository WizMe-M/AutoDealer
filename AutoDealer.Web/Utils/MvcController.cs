using Microsoft.AspNetCore.Mvc;

namespace AutoDealer.Web.Utils;

public abstract class MvcController : Controller
{
    protected readonly HttpClient ApiClient;

    protected MvcController(HttpClient client) => ApiClient = client;
}