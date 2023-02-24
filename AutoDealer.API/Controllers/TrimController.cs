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
        var model = newTrim.Construct();
        _context.Trims.Add(model);
        _context.SaveChanges();
        return Ok(model);
    }

    [HttpPatch("{id:int}/rename")]
    public IActionResult Rename(int id, string trimCode)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        found.Code = trimCode;
        _context.Trims.Update(found);

        return Ok("Model was renamed");
    }

    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        _context.Trims.Remove(found);

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