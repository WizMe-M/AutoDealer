namespace AutoDealer.Web.Controllers;

[Route("supplier")]
[Authorize(Roles = nameof(Post.PurchaseSpecialist))]
public class SupplierController : MvcController
{
    public SupplierController(ApiClient client) : base(client)
    {
    }

    [HttpGet("table")]
    public async Task<IActionResult> Table()
    {
        var clients = await Client.GetAsync<Supplier[]>("suppliers");
        return View(clients.Value ?? ArraySegment<Supplier>.Empty);
    }

    [HttpGet("create")]
    public IActionResult Create() => View();

    [HttpPost("create")]
    public async Task<IActionResult> Create(SupplierData data)
    {
        if (!ModelState.IsValid) return View(data);

        var supplier = await Client.PostAsync<SupplierData, Supplier>("suppliers/create", data);

        return RedirectToAction("Table", "Supplier");
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Client.DeleteAsync<Supplier>($"suppliers/{id}/delete");
        return RedirectToAction("Table", "Supplier");
    }
}