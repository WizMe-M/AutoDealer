namespace AutoDealer.Web.Controllers;

public class SaleController : MvcController
{
    public SaleController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Statistics(DateOnly? from, DateOnly? to)
    {
        var apiResult = await Client.GetAsync<Sale[]>($"sales/statistics?from={from}&to={to}");
        var sales = apiResult.Value ?? Array.Empty<Sale>();
        var grouped = sales.GroupBy(sale => sale.ExecutionDate).ToArray();
        return View(grouped);
    }

    [HttpGet]
    public async Task<IActionResult> Sell(int id)
    {
        var auto = await Client.GetAsync<Auto>($"autos/{id}");
        var autos = await Client.GetAsync<Auto[]>($"autos?filter={AutoStatus.Selling}");
        var clients = await Client.GetAsync<Client[]>("clients");
        var sellers = await Client.GetAsync<Employee[]>($"employees?filter={Post.Seller}");

        ViewBag.Clients = clients.Value ?? Array.Empty<Client>();
        ViewBag.Autos = autos.Value ?? Array.Empty<Auto>();
        ViewBag.Sellers = sellers.Value ?? Array.Empty<Employee>();
        var saleData = new SaleData(0, auto.Value?.Id ?? 0, 0);

        return View(saleData);
    }

    [HttpPost]
    public async Task<IActionResult> Sell(SaleData data)
    {
        if (!ModelState.IsValid) return View(data);
        var sale = await Client.PostAsync<SaleData, Sale>("sales/sell", data);
        return RedirectToAction("Table", "Auto");
    }

    [HttpGet]
    public async Task<IActionResult> Return(int id)
    {
        await Client.DeleteAsync<string>($"sales/return/{id}");
        return RedirectToAction("Table", "Auto");
    }
}