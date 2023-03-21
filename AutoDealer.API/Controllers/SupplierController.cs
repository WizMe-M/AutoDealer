namespace AutoDealer.API.Controllers;

[Authorize]
[ApiController]
[Route("suppliers")]
public class SupplierController : DbContextController<Supplier>
{
    public SupplierController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var suppliers = Context.Suppliers.ToArray();
        return Ok("All suppliers listed", suppliers);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var found = Find(id);
        return found is { }
            ? Ok("Supplier found", found)
            : Problem(detail: "Supplier with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPost("create")]
    public IActionResult Create([FromBody] SupplierData data)
    {
        var supplier = new Supplier
        {
            LegalAddress = data.Addresses.Legal,
            PostalAddress = data.Addresses.Postal,
            CorrespondentAccount = data.Accounts.Correspondent,
            SettlementAccount = data.Accounts.Settlement,
            Tin = data.Tin
        };

        Context.Suppliers.Add(supplier);
        Context.SaveChanges();

        return Ok("Supplier successfully created", supplier);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPatch("{id:int}/change-addresses")]
    public IActionResult ChangeAddress(int id, [FromBody] Addresses addresses)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "Supplier with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        found.LegalAddress = addresses.Legal;
        found.PostalAddress = addresses.Postal;

        Context.Suppliers.Update(found);
        Context.SaveChanges();

        return Ok("Supplier's addresses were updated", found);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPatch("{id:int}/change-accounts")]
    public IActionResult ChangeAccount(int id, [FromBody] Accounts accounts)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "Supplier with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        found.CorrespondentAccount = accounts.Correspondent;
        found.SettlementAccount = accounts.Settlement;

        Context.Suppliers.Update(found);
        Context.SaveChanges();

        return Ok("Supplier's accounts were updated", found);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPatch("{id:int}/change-tin")]
    public IActionResult ChangeTin(int id, [FromBody] string tin)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "Supplier with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        found.Tin = tin;

        Context.Suppliers.Update(found);
        Context.SaveChanges();

        return Ok("Supplier's tin was updated", found);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPut("{id:int}/update-data")]
    public IActionResult UpdateData(int id, [FromBody] SupplierData data)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "Supplier with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        found.LegalAddress = data.Addresses.Legal;
        found.PostalAddress = data.Addresses.Postal;
        found.CorrespondentAccount = data.Accounts.Correspondent;
        found.SettlementAccount = data.Accounts.Settlement;
        found.Tin = data.Tin;

        Context.Suppliers.Update(found);
        Context.SaveChanges();

        return Ok("Supplier data was updated", found);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "Supplier with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        Context.Suppliers.Remove(found);
        Context.SaveChanges();

        return Ok("Supplier was deleted", found);
    }

    private Supplier? Find(int id) => Context.Suppliers.FirstOrDefault(supplier => supplier.Id == id);

    protected override Task LoadReferencesAsync(Supplier entity) => throw new NotSupportedException();
}