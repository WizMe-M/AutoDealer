namespace AutoDealer.API.Controllers;

[ApiController]
[Route("contracts")]
public class ContractController : DbContextController
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
            .ToArray();

        return Ok(contracts);
    }

    [Authorize(Roles = $"{nameof(Post.PurchaseSpecialist)},{nameof(Post.Storekeeper)}")]
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var found = Find(id);
        return found is { } ? Ok(found) : NotFound("Contract with such ID doesn't exis");
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPost("create")]
    public IActionResult Create([FromBody] ContractData data)
    {
        var supplier = Context.Suppliers.FirstOrDefault(supplier => supplier.Id == data.SupplierId);
        if (supplier is null)
            return NotFound("Referenced supplier doesn't exist");

        var employee = Context.Employees.FirstOrDefault(employee => employee.Id == data.EmployeeId);
        if (employee is null)
            return NotFound("Referenced employee doesn't exist");

        var request = Context.PurchaseRequests.FirstOrDefault(request => request.Id == data.PurchaseRequestId);
        if (request is null && data.PurchaseRequestId is { })
            return NotFound("Referenced purchase request doesn't exist");

        if (!ContainsUniqueDetails(data.Details))
            return BadRequest("Contract contains duplicated details");

        if (employee is { Post: not Post.Storekeeper })
            return BadRequest("Employee should be storekeeper");

        var contract = new Contract
        {
            IdEmployee = data.EmployeeId,
            IdSupplier = data.SupplierId,
            IdPurchaseRequest = data.PurchaseRequestId,
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
        Context.SaveChanges();

        Context.Contracts.Entry(contract).Reference(e => e.Supplier).Load();
        Context.Contracts.Entry(contract).Reference(e => e.Employee).Query()
            .Include(emp => emp.User).Load();
        Context.Contracts.Entry(contract).Reference(e => e.PurchaseRequest).Query()
            .Include(req => req.User)
            .ThenInclude(u => u!.Employee)
            .Include(req => req.PurchaseRequestDetails)
            .ThenInclude(detail => detail.DetailSeries).Load();
        Context.Contracts.Entry(contract).Collection(e => e.ContractDetails).Query()
            .Include(cd => cd.DetailSeries).Load();
        Context.Contracts.Entry(contract).Collection(e => e.Details).Query()
            .Include(cd => cd.DetailSeries).Load();

        return Ok(contract);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpPatch("reschedule")]
    public IActionResult Reschedule(int id, [FromBody] DateOnly supplyDate)
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
        Context.SaveChanges();

        return Ok(found);
    }

    [Authorize(Roles = nameof(Post.PurchaseSpecialist))]
    [HttpDelete("{id:int}/break")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound("Contract with such ID doesn't exis");

        if (found.LadingBillIssueDate is { })
            return BadRequest("Can't delete finished contract");

        Context.Contracts.Remove(found);
        Context.SaveChanges();

        return Ok();
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
}