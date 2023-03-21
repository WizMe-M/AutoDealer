namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.AssemblyChief))]
[Route("car-model")]
public class CarModelController : MvcController
{
    public CarModelController(ApiClient client) : base(client)
    {
    }

    [HttpGet("table")]
    public async Task<IActionResult> Table()
    {
        var apiResult = await Client.GetAsync<CarModel[]>("car_models");
        return View(apiResult.Value ?? ArraySegment<CarModel>.Empty);
    }

    [HttpGet("create")]
    public IActionResult Create() => View();

    [HttpPost("create")]
    public async Task<IActionResult> Create(CarModelData data)
    {
        if (!ModelState.IsValid) return View(data);
        var apiResult = await Client.PostAsync<CarModelData, CarModel>("car_models/create", data);
        if (apiResult.Details is null) return RedirectToAction("Table", "CarModel");
        ModelState.AddModelError("", apiResult.Details);
        return View();
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Client.DeleteAsync<CarModel>($"car_models/{id}/delete");
        return RedirectToAction("Table", "CarModel");
    }

    [HttpGet("info/{id:int}")]
    public async Task<IActionResult> Info(int id)
    {
        var model = await Client.GetAsync<CarModel>($"car_models/{id}");
        return View(model.Value);
    }

    [HttpGet("{id:int}/assign")]
    public async Task<IActionResult> AssignDetailsToModel(int id)
    {
        var carModel = await Client.GetAsync<CarModel>($"car_models/{id}");
        if (carModel.Value is null) return RedirectToAction("Table", "CarModel");

        var detailSeries = await Client.GetAsync<DetailSeries[]>("detail_series");
        ViewBag.DetailSeries = detailSeries.Value!;
        ViewBag.CarModel = carModel.Value!;
        return View(new List<DetailCount>());
    }

    [HttpPost("{id:int}/assign")]
    public async Task<IActionResult> AssignDetailsToModel(int id, List<DetailCount> model)
    {
        if (!model.Any())
        {
            ModelState.AddModelError("", "List must contain at least one item");
            return View(model);
        }

        if (model.Any(dc => dc.Count < 1))
        {
            ModelState.AddModelError("", "List must contain at items with count more than 0");
            return View(model);
        }

        var updatedCarModel =
            await Client.PatchAsync<IEnumerable<DetailCount>, CarModel>($"car_models/{id}/set-details", model);
        return RedirectToAction("Info", "CarModel", new { id });
    }
}