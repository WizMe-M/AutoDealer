namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.AssemblyChief))]
[ApiController]
[Route("purchase_requests")]
public class PurchaseRequestController : DbContextController
{
    public PurchaseRequestController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var purchaseRequests = Context.PurchaseRequests
            .Include(request => request.User)
            .ThenInclude(user => user!.Employee)
            .Include(request => request.PurchaseRequestDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .ToArray();
        return Ok(purchaseRequests);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var found = Find(id);
        return found is { } ? Ok(found) : NotFound();
    }

    [HttpPost("create")]
    public IActionResult Create(NewPurchaseRequest newPurchaseRequest)
    {
        if (!ContainsUniqueDetails(newPurchaseRequest.DetailCountPairs))
            return BadRequest("Array of details for purchase request references on several identical details");

        var purchaseRequest = newPurchaseRequest.Construct();

        Context.PurchaseRequests.Add(purchaseRequest);
        Context.SaveChanges();
        if (purchaseRequest.IdUser is { })
        {
            Context.PurchaseRequests.Entry(purchaseRequest)
                .Reference(e => e.User).Query()
                .Include(user => user.Employee).Load();
        }

        Context.PurchaseRequests.Entry(purchaseRequest)
            .Collection(e => e.PurchaseRequestDetails).Query()
            .Include(details => details.DetailSeries).Load();

        return Ok(purchaseRequest);
    }

    [HttpPatch("{id:int}/reschedule")]
    public IActionResult Reschedule(int id, [FromBody] DateOnly supplyDate)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        var minimumSupplyDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (minimumSupplyDate >= supplyDate)
            return BadRequest("New supply date must be later than current date");

        if (found.Status is RequestStatus.Closed)
            return BadRequest("Request already closed");

        found.ExpectedSupplyDate = supplyDate;
        Context.PurchaseRequests.Update(found);
        Context.SaveChanges();

        return Ok("Purchase request's expected supply date was rescheduled");
    }

    [HttpDelete("{id:int}/cancel")]
    public IActionResult Cancel(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        if (found.Status is not RequestStatus.Sent)
            return BadRequest("Can delete only requests with status 'Sent'");

        Context.PurchaseRequests.Remove(found);
        Context.SaveChanges();

        return Ok("Purchase request was cancelled");
    }

    private PurchaseRequest? Find(int id)
    {
        return Context.PurchaseRequests
            .Include(request => request.User)
            .ThenInclude(user => user!.Employee)
            .Include(request => request.PurchaseRequestDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .FirstOrDefault(purchaseRequest => purchaseRequest.Id == id);
    }

    private static bool ContainsUniqueDetails(IEnumerable<DetailCountPair> detailCountPairs)
    {
        var id = -1;
        foreach (var (currentId, _) in detailCountPairs)
        {
            if (id == currentId) return false;
            id = currentId;
        }

        return true;
    }
}