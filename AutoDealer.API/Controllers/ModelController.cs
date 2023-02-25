namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.AssemblyChief))]
[ApiController]
[Route("models")]
public class ModelController : ControllerBase
{
    private readonly AutoDealerContext _context;

    public ModelController(AutoDealerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var models = _context.Models
            .Include(model => model.Line)
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
    public IActionResult Create(NewModel newModel)
    {
        var model = newModel.Construct();
        
        _context.Models.Add(model);
        _context.SaveChanges();
        _context.Models.Entry(model).Reference(e => e.Line).Load();
        
        return Ok(model);
    }

    [HttpPatch("{id:int}/rename")]
    public IActionResult Rename(int id, [FromBody] string modelName)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        found.Name = modelName;
        _context.Models.Update(found);
        _context.SaveChanges();
        _context.Models.Entry(found).Reference(e => e.Line).Load();

        return Ok("Model was renamed");
    }

    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        _context.Models.Remove(found);
        _context.SaveChanges();

        return Ok("Model was deleted");
    }

    private Model? Find(int id)
    {
        return _context.Models
            .Include(model => model.Line)
            .FirstOrDefault(model => model.Id == id);
    }
}