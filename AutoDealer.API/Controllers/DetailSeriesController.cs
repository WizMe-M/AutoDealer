namespace AutoDealer.API.Controllers;

[Authorize]
[ApiController]
[Route("detail_series")]
public class DetailSeriesController : DbContextController<DetailSeries>
{
    public DetailSeriesController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var detailSeries = Context.DetailSeries.ToArray();
        return Ok("All detail series listed", detailSeries);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var found = Find(id);
        return found is { }
            ? Ok("Detail series found", found)
            : NotFound("Detail's series with such ID doesn't exist");
    }

    [Authorize(Roles = $"{nameof(Post.PurchaseSpecialist)},{nameof(Post.AssemblyChief)}")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] string seriesCode)
    {
        var series = new DetailSeries { Code = seriesCode };

        Context.DetailSeries.Add(series);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(series);

        return Ok("Detail series created", series);
    }

    [Authorize(Roles = $"{nameof(Post.PurchaseSpecialist)},{nameof(Post.AssemblyChief)}")]
    [HttpPatch("{id:int}/rename")]
    public async Task<IActionResult> Rename(int id, [FromBody] string seriesCode)
    {
        var found = Find(id);
        if (found is null) return NotFound("Detail's series with such ID doesn't exist");

        found.Code = seriesCode;
        Context.DetailSeries.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Detail's series was renamed", found);
    }

    [Authorize(Roles = $"{nameof(Post.PurchaseSpecialist)},{nameof(Post.AssemblyChief)}")]
    [HttpPatch("{id:int}/change-description")]
    public async Task<IActionResult> ChangeDescription(int id, [FromBody] string description)
    {
        var found = Find(id);
        if (found is null) return NotFound("Detail's series with such ID doesn't exist");

        found.Description = description;
        Context.DetailSeries.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Detail's series description was changed", found);
    }

    [Authorize(Roles = nameof(Post.AssemblyChief))]
    [HttpDelete("{id:int}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound("Detail's series with such ID doesn't exist");

        Context.DetailSeries.Remove(found);
        await Context.SaveChangesAsync();

        return Ok("Detail's series was deleted", found);
    }

    private DetailSeries? Find(int id) => Context.DetailSeries.FirstOrDefault(series => series.Id == id);

    protected override async Task LoadReferencesAsync(DetailSeries entity)
    {
        await Context.DetailSeries.Entry(entity)
            .Collection(e => e.TrimDetails).Query()
            .Include(trimDetail => trimDetail.CarModel).LoadAsync();
    }
}