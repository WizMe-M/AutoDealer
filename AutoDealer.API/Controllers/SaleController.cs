namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.Seller))]
[ApiController]
[Route("sales")]
public class SaleController : DbContextController<Sale>
{
    public SaleController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet("statistics")]
    public IActionResult CollectStatistics([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var startDate = from?.ToDateTime() ?? DateTime.MinValue;
        var endDate = to?.ToDateTime() ?? DateTime.MaxValue;

        var sales = Context.Sales
            .Where(sale => startDate <= sale.ExecutionDate && sale.ExecutionDate <= endDate)
            .Include(sale => sale.Employee)
            .Include(sale => sale.Client)
            .Include(sale => sale.Auto)
            .ThenInclude(auto => auto.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(sale => sale.Auto)
            .ThenInclude(auto => auto.CarModel)
            .ToArray();

        return Ok("Statistics for specified period", sales);
    }

    [HttpPost("sell")]
    public async Task<IActionResult> SellAuto(SaleData saleData)
    {
        await Context.ExecuteSellAutoAsync(saleData.AutoId, saleData.ClientId, saleData.SellerId);
        var found = Context.Sales
            .Include(sale => sale.Employee)
            .Include(sale => sale.Client)
            .Include(sale => sale.Auto)
            .ThenInclude(auto => auto.CarModel)
            .ThenInclude(auto => auto.CarModelDetails)
            .ThenInclude(auto => auto.DetailSeries)
            .Include(sale => sale.Auto)
            .ThenInclude(auto => auto.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .First(sale => sale.IdAuto == saleData.AutoId);
        return Ok("Auto was successfully sold", found);
    }

    [HttpDelete("return/{autoId:int}")]
    public async Task<IActionResult> ReturnAuto(int autoId)
    {
        await Context.ExecuteReturnAutoAsync(autoId);
        return Ok("Auto was successfully returned");
    }

    protected override Task LoadReferencesAsync(Sale entity) => throw new NotSupportedException();
}