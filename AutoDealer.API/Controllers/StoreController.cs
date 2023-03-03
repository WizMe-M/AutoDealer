namespace AutoDealer.API.Controllers;

[Authorize]
[ApiController]
[Route("store")]
public class StoreController : DbContextController<Contract>
{
    public StoreController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet("lading-bills")]
    public IActionResult GetLadingBills()
    {
        var ladingBills = Context.Contracts
            .Include(contract => contract.Supplier)
            .Include(contract => contract.Storekeeper)
            .ThenInclude(employee => employee!.User)
            .Include(contract => contract.ContractDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(contract => contract.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .Where(contract => contract.LadingBillIssueDate != null)
            .ToArray();

        return Ok("All lading bills (closed contracts) listed", ladingBills);
    }

    [Authorize(Roles = nameof(Post.Storekeeper))]
    [HttpPost("contract/{contractId:int}/process")]
    public async Task<IActionResult> ProcessSupply(int contractId)
    {
        var found = Find(contractId);
        if (found is null)
            return NotFound("Contract with such ID doesn't exist");

        if (found.LadingBillIssueDate is { })
            return BadRequest("Lading bill for such supply was already processed");

        await Context.ExecuteProcessLadingBillAsync(found.Id);
        await LoadReferencesAsync(found);

        return Ok("Lading bill was successfully processed", found);
    }

    [HttpGet("details")]
    public IActionResult GetDetails()
    {
        var details = Context.Details.ToArray();
        return Ok(message: "All details", data: details);
    }

    [HttpGet("details/store")]
    public IActionResult GetDetailsInStore()
    {
        var details = Context.Details
            .Where(detail => detail.IdAuto == null)
            .ToArray();
        return Ok(message: "Details in store", data: details);
    }

    [HttpGet("details/auto")]
    public IActionResult GetUsedDetails()
    {
        var details = Context.Details
            .Where(detail => detail.IdAuto != null)
            .ToArray();
        return Ok(message: "Assembled in auto details", data: details);
    }

    private Contract? Find(int id)
    {
        return Context.Contracts
            .Include(contract => contract.Supplier)
            .Include(contract => contract.Storekeeper)
            .ThenInclude(employee => employee!.User)
            .Include(contract => contract.ContractDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(contract => contract.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .FirstOrDefault(contract => contract.Id == id);
    }

    protected override async Task LoadReferencesAsync(Contract entity)
    {
        await Context.Contracts.Entry(entity)
            .Reference(contract => contract.Supplier).LoadAsync();
        await Context.Contracts.Entry(entity)
            .Reference(contract => contract.Storekeeper).Query()
            .Include(employee => employee.User).LoadAsync();
        await Context.Contracts.Entry(entity)
            .Collection(contract => contract.ContractDetails).Query()
            .Include(detail => detail.DetailSeries).LoadAsync();
        await Context.Contracts.Entry(entity)
            .Collection(contract => contract.Details).Query()
            .Include(detail => detail.DetailSeries).LoadAsync();
    }
}