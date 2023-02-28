namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.AssemblyChief))]
[ApiController]
[Route("car_models")]
public class CarModelController : DbContextController
{
    public CarModelController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var carModels = Context.CarModels
            .Include(carModel => carModel.CarModelDetails)
            .ThenInclude(detail => detail.DetailSeries)
            .ToArray();
        return Ok(carModels);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var found = Find(id);
        return found is { } ? Ok(found) : NotFound("Car model with such ID doesn't exist");
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] CarModelData data)
    {
        var carModel = new CarModel
        {
            LineName = data.Line,
            ModelName = data.Model,
            TrimCode = data.Code
        };

        Context.CarModels.Add(carModel);
        Context.SaveChanges();
        Context.CarModels.Entry(carModel)
            .Collection(e => e.CarModelDetails).Query()
            .Include(trimDetail => trimDetail.DetailSeries).Load();

        return Ok(carModel);
    }

    [HttpPatch("{id:int}/change-model-name")]
    public IActionResult ChangeModelName(int id, [FromBody] CarModelData data)
    {
        var found = Find(id);
        if (found is null) return NotFound("Car model with such ID doesn't exist");

        found.LineName = data.Line;
        found.ModelName = data.Model;
        found.TrimCode = data.Code;

        Context.CarModels.Update(found);
        Context.SaveChanges();
        Context.CarModels.Entry(found)
            .Collection(e => e.CarModelDetails).Query()
            .Include(carModelDetail => carModelDetail.DetailSeries).Load();

        return Ok("Car model was renamed");
    }

    [HttpPatch("{id:int}/set-details")]
    public async Task<IActionResult> SetDetailsForTrim(int id, [FromBody] IEnumerable<DetailCount> detailCountPairs)
    {
        var found = Find(id);
        if (found is null) return NotFound("Car model with such ID doesn't exist");

        var details = detailCountPairs as DetailCount[] ?? detailCountPairs.ToArray();
        if (!ContainsUniqueDetails(details))
            return BadRequest("Array of details for trims references on several identical details");

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
        await Context.CarModels.Entry(found)
            .Collection(e => e.CarModelDetails).Query()
            .Include(detail => detail.DetailSeries).LoadAsync();

        return Ok(found);
    }

    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound("Car model with such ID doesn't exist");

        Context.CarModels.Remove(found);
        Context.SaveChanges();

        return Ok("Car model was deleted");
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
}