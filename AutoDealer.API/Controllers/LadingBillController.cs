namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.Storekeeper))]
[ApiController]
[Route("lading_bills")]
public class LadingBillController : DbContextController
{
    public LadingBillController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetLadingBills()
    {
        var ladingBills = Context.Contracts
            .Include(contract => contract.Supplier)
            .Include(contract => contract.Employee)
            .ThenInclude(employee => employee!.User)
            .Include(contract => contract.PurchaseRequest)
            .ThenInclude(request => request!.PurchaseRequestDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(contract => contract.PurchaseRequest)
            .ThenInclude(request => request!.User)
            .ThenInclude(detail => detail!.Employee)
            .Include(contract => contract.ContractDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(contract => contract.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .Where(contract => contract.LadingBillIssueDate != null)
            .ToArray();

        return Ok(ladingBills);
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

        return Ok("Lading bill was successfully processed");
    }

    private Contract? Find(int id)
    {
        return Context.Contracts
            .Include(contract => contract.Supplier)
            .Include(contract => contract.Employee)
            .ThenInclude(employee => employee!.User)
            .Include(contract => contract.PurchaseRequest)
            .ThenInclude(request => request!.PurchaseRequestDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(contract => contract.PurchaseRequest)
            .ThenInclude(request => request!.User)
            .ThenInclude(detail => detail!.Employee)
            .Include(contract => contract.ContractDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(contract => contract.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .FirstOrDefault(contract => contract.Id == id);
    }
}