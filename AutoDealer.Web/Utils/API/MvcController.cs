namespace AutoDealer.Web.Utils.API;

public abstract class MvcController : Controller
{
    protected readonly ApiClient Client;

    protected MvcController(ApiClient client)
    {
        Client = client;
    }
}