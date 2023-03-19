namespace AutoDealer.API.Controllers;

[Authorize]
[ApiController]
[Route("car_models")]
public class CarModelController : DbContextController<CarModel>
{
    public CarModelController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll(CarModelSort? sort)
    {
        var carModels = Context.CarModels
            .Include(carModel => carModel.CarModelDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .ToArray();

        carModels = (sort switch
        {
            null or CarModelSort.IdAsc => carModels.OrderBy(model => model.Id),
            CarModelSort.IdDesc => carModels.OrderByDescending(model => model.Id),
            CarModelSort.LineAsc => carModels.OrderBy(model => model.LineName),
            CarModelSort.LineDesc => carModels.OrderByDescending(model => model.LineName),
            CarModelSort.ModelAsc => carModels.OrderBy(model => model.ModelName),
            CarModelSort.ModelDesc => carModels.OrderByDescending(model => model.ModelName),
            CarModelSort.CodeAsc => carModels.OrderBy(model => model.TrimCode),
            CarModelSort.CodeDesc => carModels.OrderByDescending(model => model.TrimCode),
            _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
        }).ToArray();

        return Ok("Car models listed", carModels);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var found = Find(id);
        return found is { }
            ? Ok("Car model found", found)
            : Problem(detail: "Car model with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);
    }

    [Authorize(Roles = nameof(Post.AssemblyChief))]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CarModelData data)
    {
        var carModel = new CarModel
        {
            LineName = data.Line,
            ModelName = data.Model,
            TrimCode = data.Code
        };

        Context.CarModels.Add(carModel);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(carModel);

        return Ok("Car model created", carModel);
    }

    [Authorize(Roles = nameof(Post.AssemblyChief))]
    [HttpPatch("{id:int}/change-model-name")]
    public async Task<IActionResult> ChangeModelName(int id, [FromBody] CarModelData data)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "Car model with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        found.LineName = data.Line;
        found.ModelName = data.Model;
        found.TrimCode = data.Code;

        Context.CarModels.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Car model was renamed", found);
    }

    [Authorize(Roles = nameof(Post.AssemblyChief))]
    [HttpPatch("{id:int}/set-details")]
    public async Task<IActionResult> SetDetailsForTrim(int id, [FromBody] IEnumerable<DetailCount> detailCountPairs)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "Car model with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        var details = detailCountPairs as DetailCount[] ?? detailCountPairs.ToArray();
        if (!ContainsUniqueDetails(details))
            return Problem(detail: "Array of details for trims references on several identical details",
                statusCode: StatusCodes.Status400BadRequest);

        found.CarModelDetails.Clear();

        foreach (var (seriesId, count) in details)
        {
            found.CarModelDetails.Add(new CarModelDetail
            {
                IdCarModel = id,
                IdDetailSeries = seriesId,
                Count = count
            });
        }

        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Details for car model were set", found);
    }

    [Authorize(Roles = nameof(Post.AssemblyChief))]
    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "Car model with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        Context.CarModels.Remove(found);
        Context.SaveChanges();

        return Ok("Car model was deleted", found);
    }

    private CarModel? Find(int id)
    {
        return Context.CarModels
            .Include(trim => trim.CarModelDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .FirstOrDefault(trim => trim.Id == id);
    }

    private static bool ContainsUniqueDetails(IEnumerable<DetailCount> detailInTrims)
    {
        var id = -1;
        foreach (var (currentId, _) in detailInTrims)
        {
            if (id == currentId) return false;
            id = currentId;
        }

        return true;
    }

    protected override async Task LoadReferencesAsync(CarModel entity)
    {
        await Context.CarModels.Entry(entity)
            .Collection(e => e.CarModelDetails).Query()
            .Include(detail => detail.DetailSeries).LoadAsync();
    }
}