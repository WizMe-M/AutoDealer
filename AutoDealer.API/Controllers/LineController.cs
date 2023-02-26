namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.AssemblyChief))]
[ApiController]
[Route("lines")]
public class LineController : ControllerBase
{
    private readonly AutoDealerContext _context;

    public LineController(AutoDealerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var lines = _context.Lines.ToArray();
        return Ok(lines);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var found = FindById(id);
        return found is { } ? Ok(found) : NotFound();
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] string lineName)
    {
        var line = new Line { Name = lineName };
        _context.Lines.Add(line);
        _context.SaveChanges();
        return Ok(line);
    }

    [HttpPatch("{id:int}/rename")]
    public IActionResult Rename(int id, [FromBody] string lineName)
    {
        var found = FindById(id);
        if (found is null) return NotFound();

        found.Name = lineName;
        _context.Lines.Update(found);

        return Ok("Line was renamed");
    }

    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = FindById(id);
        if (found is null) return NotFound();

        _context.Lines.Remove(found);

        return Ok("Line was deleted");
    }

    private Line? FindById(int id) => _context.Lines.FirstOrDefault(line => line.Id == id);
}