namespace AutoDealer.API.Controllers;

[ApiController]
[Route("margins")]
public class MarginController : DbContextController<Margin>
{
    public MarginController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var margins = Context.Margins
            .Include(margin => margin.CarModel)
            .ToArray();

        return Ok("All margins listed", margins);
    }

    [HttpGet("{carModelId:int}")]
    public IActionResult GetMarginsForModel(int carModelId)
    {
        var margins = Context.Margins
            .Where(margin => margin.IdCarModel == carModelId)
            .Include(margin => margin.CarModel)
            .ToArray();

        return Ok($"Margins for car model with ID {carModelId} listed", margins);
    }

    [HttpGet("get-in-range")]
    public IActionResult GetMarginsInRange(DateOnly? from, DateOnly? to)
    {
        var startDate = from ?? DateOnly.MinValue;
        var endDate = to ?? DateOnly.MaxValue;

        var margins = Context.Margins
            .Where(margin => startDate <= margin.StartDate && margin.StartDate <= endDate)
            .Include(margin => margin.CarModel).ToArray();

        return Ok($"Margins in range from {from} to {to} listed", margins);
    }

    [HttpPost("set-margin")]
    public async Task<IActionResult> SetMargin([FromBody] MarginData data)
    {
        await Context.ExecuteSetMarginAsync(data.CarModelId, data.StartsFrom, data.MarginValue);

        var created = await Context.Margins.FirstAsync(margin =>
            margin.IdCarModel == data.CarModelId &&
            margin.StartDate == data.StartsFrom);
        await LoadReferencesAsync(created);

        return Ok("Margin was successfully set", created);
    }

    [HttpDelete("delete")]
    public IActionResult Delete([FromBody] MarginIdentifier identifier)
    {
        var found = Context.Margins.FirstOrDefault(margin =>
            margin.IdCarModel == identifier.CarModelId &&
            margin.StartDate == identifier.StartsFrom);

        if (found is null)
            return NotFound("Margin with such identifying data can't be found");

        Context.Margins.Remove(found);
        Context.SaveChanges();

        return Ok("Margin was successfully deleted", found);
    }

    protected override async Task LoadReferencesAsync(Margin entity)
    {
        await Context.Margins.Entry(entity)
            .Reference(margin => margin.CarModel).Query()
            .Include(model => model.CarModelDetails)
            .ThenInclude(detail => detail.DetailSeries).LoadAsync();
    }
}