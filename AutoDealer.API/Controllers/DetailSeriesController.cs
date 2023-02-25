namespace AutoDealer.API.Controllers;

[ApiController]
[Route("detail_series")]
public class DetailSeriesController : ControllerBase
{
    private readonly AutoDealerContext _context;

    public DetailSeriesController(AutoDealerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var detailSeries = _context.DetailSeries.ToArray();
        return Ok(detailSeries);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var found = FindById(id);
        return found is { } ? Ok(found) : NotFound();
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] string seriesCode)
    {
        var series = new DetailSeries { Code = seriesCode };
        
        _context.DetailSeries.Add(series);
        _context.SaveChanges();
        _context.DetailSeries.Entry(series)
            .Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.Trim)
            .ThenInclude(trim => trim.Model)
            .ThenInclude(model => model.Line).Load();
        
        return Ok(series);
    }

    [HttpPatch("{id:int}/rename")]
    public IActionResult Rename(int id, [FromBody] string seriesCode)
    {
        var found = FindById(id);
        if (found is null) return NotFound();

        found.Code = seriesCode;
        _context.DetailSeries.Update(found);
        _context.SaveChanges();
        _context.DetailSeries.Entry(found)
            .Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.Trim)
            .ThenInclude(trim => trim.Model)
            .ThenInclude(model => model.Line).Load();

        return Ok("Detail's series was renamed");
    }

    [HttpPatch("{id:int}/change-description")]
    public IActionResult ChangeDescription(int id, [FromBody] string description)
    {
        var found = FindById(id);
        if (found is null) return NotFound();

        found.Description = description;
        _context.DetailSeries.Update(found);
        _context.SaveChanges();
        _context.DetailSeries.Entry(found)
            .Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.Trim)
            .ThenInclude(trim => trim.Model)
            .ThenInclude(model => model.Line).Load();

        return Ok("Detail's series was renamed");
    }

    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = FindById(id);
        if (found is null) return NotFound();

        _context.DetailSeries.Remove(found);
        _context.SaveChanges();

        return Ok("Detail's series was deleted");
    }

    private DetailSeries? FindById(int id) => _context.DetailSeries.FirstOrDefault(series => series.Id == id);
}