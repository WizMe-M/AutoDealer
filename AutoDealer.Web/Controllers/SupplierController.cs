namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.PurchaseSpecialist))]
public class SupplierController : MvcController
{
    public SupplierController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Table()
    {
        var clients = await Client.GetAsync<Supplier[]>("suppliers");
        return View(clients.Value ?? ArraySegment<Supplier>.Empty);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(SupplierData data)
    {
        if (!ModelState.IsValid) return View(data);

        var supplier = await Client.PostAsync<SupplierData, Supplier>("suppliers/create", data);

        return RedirectToAction("Table", "Supplier");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        await Client.DeleteAsync<Supplier>($"suppliers/{id}/delete");
        return RedirectToAction("Table", "Supplier");
    }
}