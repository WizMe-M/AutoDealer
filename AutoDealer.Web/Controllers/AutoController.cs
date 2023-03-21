namespace AutoDealer.Web.Controllers;

public class AutoController : MvcController
{
    public AutoController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Table()
    {
        var autos = await Client.GetAsync<Auto[]>("autos");
        return View(autos.Value ?? ArraySegment<Auto>.Empty);
    }

    [HttpGet]
    public async Task<IActionResult> Info(int id)
    {
        var auto = await Client.GetAsync<Auto>($"autos/{id}");
        if (auto.Value is null) return RedirectToAction("Table", "CarModel");
        return View(auto.Value);
    }
}