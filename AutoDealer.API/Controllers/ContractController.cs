namespace AutoDealer.API.Controllers;

[ApiController]
[Route("contracts")]
public class ContractController : DbContextController<Contract>
{
    public ContractController(AutoDealerContext context) : base(context)
    {
    }

    [Authorize(Roles = $"{nameof(Post.PurchaseSpecialist)},{nameof(Post.Storekeeper)}")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var contracts = Context.Contracts
            .Include(contract => contract.Supplier)
            .Include(contract => contract.Storekeeper)
            .ThenInclude(employee => employee!.User)
            .Include(contract => contract.ContractDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(contract => contract.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .ToArray();

        return Ok("All contracts listed", contracts);
    }

    [Authorize(Roles = $"{nameof(Post.PurchaseSpecialist)},{nameof(Post.Storekeeper)}")]
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var found = Find(id);
        return found is { }
            ? Ok("Contract found", found)
            : NotFound("Contract with such ID doesn't exist");
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ContractData data)
    {
        var supplier = Context.Suppliers.FirstOrDefault(supplier => supplier.Id == data.SupplierId);
        if (supplier is null)
            return NotFound("Referenced supplier doesn't exist");

        var employee = Context.Employees.FirstOrDefault(employee => employee.Id == data.EmployeeId);
        if (employee is null)
            return NotFound("Referenced employee doesn't exist");

        if (!ContainsUniqueDetails(data.Details))
            return BadRequest("Contract contains duplicated details");

        if (employee is { Post: not Post.Storekeeper })
            return BadRequest("Employee should be storekeeper");

        var contract = new Contract
        {
            IdEmployee = data.EmployeeId,
            IdSupplier = data.SupplierId,
            SupplyDate = data.SupplyDate
        };

        var sum = 0m;
        foreach (var (series, count, cost) in data.Details)
        {
            contract.ContractDetails.Add(
                new ContractDetail
                {
                    IdDetailSeries = series,
                    Count = count,
                    CostPerOne = cost
                });
            sum += count * cost;
        }

        contract.TotalSum = sum;

        Context.Contracts.Add(contract);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(contract);

        return Ok("Contract was processed", contract);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPatch("reschedule")]
    public async Task<IActionResult> Reschedule(int id, [FromBody] DateOnly supplyDate)
    {
        var found = Find(id);
        if (found is null) return NotFound("Contract with such ID doesn't exist");

        if (found.LadingBillIssueDate is { })
            return BadRequest("Can't reschedule finished contract");

        var minimalSupplyDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (supplyDate <= minimalSupplyDate)
            return BadRequest("Supply date should be later than now");

        found.SupplyDate = supplyDate;
        Context.Contracts.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Contract was rescheduled", found);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpDelete("{id:int}/break")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound("Contract with such ID doesn't exist");

        if (found.LadingBillIssueDate is { })
            return BadRequest("Can't delete finished contract");

        Context.Contracts.Remove(found);
        Context.SaveChanges();

        return Ok("Contract was deleted", found);
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

    private static bool ContainsUniqueDetails(IEnumerable<DetailCountCost> details)
    {
        var id = -1;
        foreach (var (currentId, _, _) in details)
        {
            if (id == currentId) return false;
            id = currentId;
        }

        return true;
    }

    protected override async Task LoadReferencesAsync(Contract entity)
    {
        await Context.Contracts.Entry(entity)
            .Reference(e => e.Supplier).LoadAsync();
        await Context.Contracts.Entry(entity)
            .Reference(e => e.Storekeeper).Query()
            .Include(emp => emp.User).LoadAsync();
        await Context.Contracts.Entry(entity)
            .Collection(e => e.ContractDetails).Query()
            .Include(cd => cd.DetailSeries).LoadAsync();
        await Context.Contracts.Entry(entity)
            .Collection(e => e.Details).Query()
            .Include(cd => cd.DetailSeries).LoadAsync();
    }
}