namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.Storekeeper))]
[ApiController]
[Route("lading_bills")]
public class LadingBillController : DbContextController<Contract>
{
    public LadingBillController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
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

    [HttpPost("{contractId:int}")]
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