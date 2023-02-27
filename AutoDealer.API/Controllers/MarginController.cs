namespace AutoDealer.API.Controllers;

[ApiController]
[Route("margins")]
public class MarginController : DbContextController
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
        return Ok(margins);
    }

    [HttpGet("{carModelId:int}")]
    public IActionResult GetMarginsForModel(int carModelId)
    {
        var margins = Context.Margins
            .Where(margin => margin.IdCarModel == carModelId)
            .Include(margin => margin.CarModel)
            .ToArray();
        return Ok(margins);
    }

    [HttpGet("get-in-range")]
    public IActionResult GetMarginsInRange(DateOnly from, DateOnly? to)
    {
        var endDate = to ?? DateOnly.MaxValue;

        var margins = Context.Margins
            .Where(margin => from <= margin.StartDate && margin.StartDate <= endDate)
            .Include(margin => margin.CarModel)
            .ToArray();
        return Ok(margins);
    }

    [HttpPost("set-margin")]
    public async Task<IActionResult> SetMargin([FromBody] MarginData marginData)
    {
        await Context.ExecuteSetMarginAsync(marginData.CarModelId, marginData.StartsFrom, marginData.MarginValue);

        return Ok("Margin was successfully set");
    }

    [HttpDelete("delete")]
    public IActionResult Delete([FromBody] MarginIdentifier identifier)
    {
        var found = Context.Margins.FirstOrDefault(margin =>
            margin.IdCarModel == identifier.CarModelId &&
            margin.StartDate == identifier.StartsFrom);

        if (found is null) return NotFound();

        Context.Margins.Remove(found);
        Context.SaveChanges();

        return Ok("Margin was successfully deleted");
    }
}