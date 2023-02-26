namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.PurchaseSpecialist))]
[ApiController]
[Route("suppliers")]
public class SupplierController : DbContextController
{
    public SupplierController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var suppliers = Context.Suppliers.ToArray();
        return Ok(suppliers);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var found = Find(id);
        return found is { } ? Ok(found) : NotFound();
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] SupplierData data)
    {
        var supplier = data.Construct();

        Context.Suppliers.Add(supplier);
        Context.SaveChanges();

        return Ok(supplier);
    }

    [HttpPut("{id:int}/change-data")]
    public IActionResult ChangeData(int id, [FromBody] SupplierData data)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        var supplier = data.Construct();
        found.LegalAddress = supplier.LegalAddress;
        found.PostalAddress = supplier.PostalAddress;
        found.CorrespondentAccount = supplier.CorrespondentAccount;
        found.SettlementAccount = supplier.SettlementAccount;
        found.Tin = supplier.Tin;
        Context.Suppliers.Update(found);
        Context.SaveChanges();

        return Ok("Supplier data was updated");
    }

    [HttpDelete("delete")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        Context.Suppliers.Remove(found);
        Context.SaveChanges();

        return Ok("Supplier was deleted");
    }

    private Supplier? Find(int id) => Context.Suppliers.FirstOrDefault(supplier => supplier.Id == id);
}