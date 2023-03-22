namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.Storekeeper))]
public class StoreController : MvcController
{
    public StoreController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Details(DetailSort sort = DetailSort.IdAsc)
    {
        ViewBag.IdSort = sort is DetailSort.IdAsc ? DetailSort.IdDesc : DetailSort.IdAsc;
        ViewBag.CostSort = sort is DetailSort.CostAsc ? DetailSort.CostDesc : DetailSort.CostAsc;

        var result = await Client.GetAsync<Detail[]>($"store/details?sort={sort}");
        return View(result.Value ?? Array.Empty<Detail>());
    }

    [HttpGet]
    public async Task<IActionResult> Process(int id)
    {
        var ladingBill = await Client.PostAsync<string, Contract>(
            $"store/contract/{id}/process", string.Empty);

        if (ladingBill.Value is null)
            return RedirectToAction("Table", "Contract");

        return RedirectToAction("LadingBill", new { id });
    }

    [HttpGet]
    public async Task<IActionResult> LadingBill(int id)
    {
        var bill = await Client.GetAsync<Contract>($"contracts/{id}");
        if (bill.Value is null) return RedirectToAction("Table", "Contract");
        return View(bill.Value);
    }
}