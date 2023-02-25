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
        var models = _context.Trims
            .Include(trim => trim.Model)
            .ThenInclude(model => model.Line)
            .ToArray();
        return Ok(models);
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
        _context.Trims.Entry(found) .Collection(e => e.TrimDetails).Query()
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

    private Trim? Find(int id)
    {
        return _context.Trims
            .Include(trim => trim.Model)
            .ThenInclude(model => model.Line)
            .FirstOrDefault(trim => trim.Id == id);
    }
}