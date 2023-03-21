namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.PurchaseSpecialist))]
public class ContractController : MvcController
{
    public ContractController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Table()
    {
        var contracts = await Client.GetAsync<Contract[]>("contracts");
        return View(contracts.Value ?? ArraySegment<Contract>.Empty);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var suppliers = await Client.GetAsync<Supplier[]>("suppliers");
        var storekeepers = await Client.GetAsync<Employee[]>($"employees?filter={Post.Storekeeper}");
        var detailsSeries = await Client.GetAsync<DetailSeries[]>("detail_series");
        ViewBag.Suppliers = suppliers.Value!;
        ViewBag.Storekeepers = storekeepers.Value!;
        ViewBag.DetailSeries = detailsSeries.Value!;
        var data = new ContractData(0, 0, DateOnly.MinValue, new List<DetailCountCost>());
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContractData data)
    {
        if (!ModelState.IsValid) return View(data);
        
        if (!data.Details.Any())
        {
            ModelState.AddModelError("", "List must contain at least one item");
            return View(data);
        }

        if (data.Details.Any(dc => dc.Count < 1))
        {
            ModelState.AddModelError("", "List must contain items with count more than 0");
            return View(data);
        }

        if (data.Details.Any(dc => dc.CostPerOne < 1))
        {
            ModelState.AddModelError("", "List must contain items with cost more than 0");
            return View(data);
        }
        
        
        var created = await Client.PostAsync<ContractData, Contract>("contracts/create", data);
        return RedirectToAction("Table", "Contract");
    }

    [HttpGet]
    public async Task<IActionResult> Info(int id)
    {
        var contract = await Client.GetAsync<Contract>($"contracts/{id}");
        if (contract.Value is null)
            return RedirectToAction("Info", "Contract");
        return View(contract.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await Client.DeleteAsync<Contract>($"contracts/{id}/break");
        return RedirectToAction("Table", "Contract");
    }
}