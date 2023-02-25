namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.AssemblyChief))]
[ApiController]
[Route("trims")]
public class TrimController : ControllerBase
{
    private readonly AutoDealerContext _context;

    public TrimController(AutoDealerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var trims = _context.Trims
            .Include(trim => trim.Model)
            .ThenInclude(model => model.Line)
            .Include(trim => trim.TrimDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .ToArray();
        return Ok(trims);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var found = Find(id);
        return found is { } ? Ok(found) : NotFound();
    }

    [HttpPost("create")]
    public IActionResult Create(NewTrim newTrim)
    {
        var trim = newTrim.Construct();

        _context.Trims.Add(trim);
        _context.SaveChanges();
        _context.Trims.Entry(trim).Reference(e => e.Model).Load();
        _context.Trims.Entry(trim).Reference(e => e.Model).Query()
            .Include(m => m.Line).Load();
        _context.Trims.Entry(trim).Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.DetailSeries).Load();
        return Ok(trim);
    }

    [HttpPatch("{id:int}/rename")]
    public IActionResult Rename(int id, [FromBody] string trimCode)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        found.Code = trimCode;
        _context.Trims.Update(found);
        _context.SaveChanges();
        _context.Trims.Entry(found).Reference(e => e.Model).Load();
        _context.Trims.Entry(found).Reference(e => e.Model).Query()
            .Include(m => m.Line).Load();
        _context.Trims.Entry(found).Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.DetailSeries).Load();

        return Ok("Model was renamed");
    }

    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        _context.Trims.Remove(found);
        _context.SaveChanges();

        return Ok("Model was deleted");
    }

    [HttpPatch("{id:int}/set-details")]
    public async Task<IActionResult> SetDetailsForTrim(int id, [FromBody] IEnumerable<DetailCountPair> detailCountPairs)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        var details = detailCountPairs as DetailCountPair[] ?? detailCountPairs.ToArray();
        if (!ContainsUniqueDetails(details))
            return BadRequest("Array of details for trims references on several identical details");

        found.TrimDetails.Clear();

        foreach (var (seriesId, count) in details)
        {
            found.TrimDetails.Add(new TrimDetail
            {
                IdTrim = id,
                IdDetailSeries = seriesId,
                Count = count
            });
        }

        await _context.SaveChangesAsync();
        await _context.Trims.Entry(found)
            .Collection(e => e.TrimDetails).Query()
            .Include(detail => detail.DetailSeries).LoadAsync();

        return Ok(found);
    }

    private Trim? Find(int id)
    {
        return _context.Trims
            .Include(trim => trim.Model)
            .ThenInclude(model => model.Line)
            .Include(trim => trim.TrimDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .FirstOrDefault(trim => trim.Id == id);
    }

    private static bool ContainsUniqueDetails(IEnumerable<DetailCountPair> detailInTrims)
    {
        var id = -1;
        foreach (var (currentId, _) in detailInTrims)
        {
            if (id == currentId) return false;
            id = currentId;
        }

        return true;
    }
}