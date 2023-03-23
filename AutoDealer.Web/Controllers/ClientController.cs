namespace AutoDealer.Web.Controllers;

public class ClientController : MvcController
{
    public ClientController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Table()
    {
        var clients = await Client.GetAsync<Client[]>("clients");
        return View(clients.Value ?? Array.Empty<Client>());
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ClientData data)
    {
        if (!ModelState.IsValid) return View(data);
        
        var client = await Client.PostAsync<ClientData, Client>("clients/create", data);
        if (client.Details is null) return RedirectToAction("Table", "Client");
        
        ModelState.AddModelError("", client.Details);
        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> Info(int id)
    {
        var client = await Client.GetAsync<Client>($"clients/{id}");
        if (client.Value is null) return RedirectToAction("Table", "Client");
        return View(client.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await Client.DeleteAsync<Client>($"clients/{id}/delete");
        return RedirectToAction("Table", "Client");
    }
}