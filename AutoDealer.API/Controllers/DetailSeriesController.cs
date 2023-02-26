namespace AutoDealer.API.Controllers;

[ApiController]
[Route("detail_series")]
public class DetailSeriesController : DbContextController
{
    public DetailSeriesController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var detailSeries = Context.DetailSeries.ToArray();
        return Ok(detailSeries);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var found = Find(id);
        return found is { } ? Ok(found) : NotFound();
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] string seriesCode)
    {
        var series = new DetailSeries { Code = seriesCode };
        
        Context.DetailSeries.Add(series);
        Context.SaveChanges();
        Context.DetailSeries.Entry(series)
            .Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.CarModel).Load();
        
        return Ok(series);
    }

    [HttpPatch("{id:int}/rename")]
    public IActionResult Rename(int id, [FromBody] string seriesCode)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        found.Code = seriesCode;
        Context.DetailSeries.Update(found);
        Context.SaveChanges();
        Context.DetailSeries.Entry(found)
            .Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.CarModel).Load();

        return Ok("Detail's series was renamed");
    }

    [HttpPatch("{id:int}/change-description")]
    public IActionResult ChangeDescription(int id, [FromBody] string description)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        found.Description = description;
        Context.DetailSeries.Update(found);
        Context.SaveChanges();
        Context.DetailSeries.Entry(found)
            .Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.CarModel).Load();

        return Ok("Detail's series description was changed");
    }

    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound();

        Context.DetailSeries.Remove(found);
        Context.SaveChanges();

        return Ok("Detail's series was deleted");
    }

    private DetailSeries? Find(int id) => Context.DetailSeries.FirstOrDefault(series => series.Id == id);
}