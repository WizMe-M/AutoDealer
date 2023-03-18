using Microsoft.AspNetCore.Mvc;

namespace AutoDealer.Web.Utils;

public abstract class MvcController : Controller
{
    protected readonly HttpClient Client;

    protected MvcController(HttpClient client)
    {
        Client = client;
        Client.BaseAddress = new Uri("https://api:44357/api/");
    }
}