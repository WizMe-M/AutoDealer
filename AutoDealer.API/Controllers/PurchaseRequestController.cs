namespace AutoDealer.API.Controllers;

[Authorize]
[ApiController]
[Route("purchase_requests")]
public class PurchaseRequestController : DbContextController<PurchaseRequest>
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
        return Ok("All purchase request listed", purchaseRequests);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var found = Find(id);
        return found is { }
            ? Ok("Purchase request found", found)
            : NotFound("Purchase request with such ID doesn't exist");
    }

    [Authorize(Roles = nameof(Post.AssemblyChief))]
    [HttpPost("create")]
    public async Task<IActionResult> Create(PurchaseRequestData data)
    {
        if (!ContainsUniqueDetails(data.DetailCounts))
            return BadRequest("Array of details for purchase request references on several identical details");

        var purchaseRequest = new PurchaseRequest
        {
            ExpectedSupplyDate = data.ExpectedSupplyDate,
            IdUser = data.IdUser
        };

        foreach (var (seriesId, count) in data.DetailCounts)
        {
            purchaseRequest.PurchaseRequestDetails.Add(
                new PurchaseRequestDetail
                {
                    IdDetailSeries = seriesId,
                    Count = count
                });
        }

        Context.PurchaseRequests.Add(purchaseRequest);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(purchaseRequest);

        return Ok("Purchase request was sent", purchaseRequest);
    }

    [Authorize(Roles = nameof(Post.AssemblyChief))]
    [HttpPatch("{id:int}/reschedule")]
    public async Task<IActionResult> Reschedule(int id, [FromBody] DateOnly supplyDate)
    {
        var found = Find(id);
        if (found is null) return NotFound("Purchase request with such ID doesn't exist");

        var minimumSupplyDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (minimumSupplyDate >= supplyDate)
            return BadRequest("New supply date must be later than current date");

        if (found.Status is RequestStatus.Closed)
            return BadRequest("Request already closed");

        found.ExpectedSupplyDate = supplyDate;
        Context.PurchaseRequests.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Purchase request's expected supply date was rescheduled", found);
    }

    [Authorize(Roles = nameof(Post.AssemblyChief))]
    [HttpDelete("{id:int}/cancel")]
    public IActionResult Cancel(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound("Purchase request with such ID doesn't exist");

        if (found.Status is not RequestStatus.Sent)
            return BadRequest("Can delete only requests with status 'Sent'");

        Context.PurchaseRequests.Remove(found);
        Context.SaveChanges();

        return Ok("Purchase request was cancelled", found);
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

    private static bool ContainsUniqueDetails(IEnumerable<DetailCount> detailCountPairs)
    {
        var id = -1;
        foreach (var (currentId, _) in detailCountPairs)
        {
            if (id == currentId) return false;
            id = currentId;
        }

        return true;
    }

    protected override async Task LoadReferencesAsync(PurchaseRequest entity)
    {
        await Context.PurchaseRequests.Entry(entity)
            .Reference(e => e.User).Query()
            .Include(user => user.Employee).LoadAsync();
        await Context.PurchaseRequests.Entry(entity)
            .Collection(e => e.PurchaseRequestDetails).Query()
            .Include(details => details.DetailSeries).LoadAsync();
    }
}