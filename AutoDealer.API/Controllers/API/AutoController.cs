using AutoDealer.API.Sort;

namespace AutoDealer.API.Controllers.API;

[ApiController]
[Route("autos")]
public class AutoController : DbContextController<Auto>
{
    public AutoController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll(AutoSort? sort, AutoStatus? filter)
    {
        Auto[] autos;
        if (filter is { })
        {
            autos = Context.Autos
                .Include(auto => auto.CarModel)
                .Include(auto => auto.Details)
                .ThenInclude(detail => detail.DetailSeries)
                .Where(auto => auto.Status == filter)
                .ToArray();
        }
        else
        {
            autos = Context.Autos
                .Include(auto => auto.CarModel)
                .Include(auto => auto.Details)
                .ThenInclude(detail => detail.DetailSeries)
                .ToArray();
        }

        autos = (sort switch
        {
            null or AutoSort.IdAsc => autos.OrderBy(auto => auto.Id),
            AutoSort.IdDesc => autos.OrderByDescending(auto => auto.Id),
            AutoSort.AssemblyDateAsc => autos.OrderBy(auto => auto.AssemblyDate),
            AutoSort.AssemblyDateDesc => autos.OrderByDescending(auto => auto.AssemblyDate),
            AutoSort.CostAsc => autos.OrderBy(auto => auto.Cost),
            AutoSort.CostDesc => autos.OrderByDescending(auto => auto.Cost),
            _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
        }).ToArray();

        return Ok("All autos listed", autos);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var found = Context.Autos.FirstOrDefault(auto => auto.Id == id);
        return found is { }
            ? Ok("Found auto", found)
            : Problem(detail: "Auto with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);
    }

    [HttpPost("assembly/{carModelId:int}")]
    public async Task<IActionResult> AssemblyAuto(int carModelId)
    {
        var model = Context.CarModels
            .Include(carModel => carModel.CarModelDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .FirstOrDefault(carModel => carModel.Id == carModelId);

        if (model is null)
            return Problem(detail: "Car model with such ID doesn't found", statusCode: StatusCodes.Status404NotFound);

        var details = new List<Detail>();

        var isMissing = false;
        var missingDetails = new Dictionary<CarModelDetail, int>();

        foreach (var carModelDetail in model.CarModelDetails)
        {
            var detailsForModel = Context.Details
                .Where(detail => detail.IdDetailSeries == carModelDetail.IdDetailSeries)
                .Take(carModelDetail.Count)
                .ToArray();
            if (detailsForModel.Length < carModelDetail.Count)
            {
                isMissing = true;
                var missingCount = carModelDetail.Count - detailsForModel.Length;
                missingDetails.Add(carModelDetail, missingCount);
            }

            if (!isMissing) details.AddRange(detailsForModel);
        }

        if (isMissing)
        {
            var missings = new List<dynamic>();
            foreach (var (key, value) in missingDetails)
            {
                missings.Add(new { count = value, detail = key });
            }

            var error = new
            {
                message = "Not enough details",
                missedDetails = missings
            }.ToString();
            return Problem(detail: error, statusCode: StatusCodes.Status400BadRequest);
        }

        var detailsTotalCost = details.Select(detail => detail.Cost).Sum();

        var auto = new Auto
        {
            IdCarModel = carModelId,
            Cost = detailsTotalCost
        };

        foreach (var detail in details) auto.Details.Add(detail);

        Context.Autos.Add(auto);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(auto);

        return Ok("Auto was assembled", auto);
    }

    protected override async Task LoadReferencesAsync(Auto entity)
    {
        await Context.Autos.Entry(entity)
            .Reference(auto => auto.CarModel).LoadAsync();
        await Context.Autos.Entry(entity)
            .Collection(auto => auto.Details).Query()
            .Include(detail => detail.DetailSeries).LoadAsync();
    }
}