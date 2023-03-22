namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.AssemblyChief))]
public class CarModelController : MvcController
{
    public CarModelController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Table()
    {
        var apiResult = await Client.GetAsync<CarModel[]>("car_models");
        return View(apiResult.Value ?? Array.Empty<CarModel>());
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CarModelData data)
    {
        if (!ModelState.IsValid) return View(data);
        var apiResult = await Client.PostAsync<CarModelData, CarModel>("car_models/create", data);
        if (apiResult.Details is null) return RedirectToAction("Table", "CarModel");
        ModelState.AddModelError("", apiResult.Details);
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        await Client.DeleteAsync<CarModel>($"car_models/{id}/delete");
        return RedirectToAction("Table", "CarModel");
    }

    [HttpGet]
    public async Task<IActionResult> Info(int id)
    {
        var model = await Client.GetAsync<CarModel>($"car_models/{id}");
        return View(model.Value);
    }

    [HttpGet]
    public async Task<IActionResult> AssignDetailsToModel(int id)
    {
        var carModel = await Client.GetAsync<CarModel>($"car_models/{id}");
        if (carModel.Value is null) return RedirectToAction("Table", "CarModel");

        var detailSeries = await Client.GetAsync<DetailSeries[]>("detail_series");
        ViewBag.DetailSeries = detailSeries.Value!;
        ViewBag.CarModel = carModel.Value!;
        return View(new List<DetailCount>());
    }

    [HttpPost]
    public async Task<IActionResult> AssignDetailsToModel(int id, List<DetailCount> model)
    {
        if (!model.Any())
        {
            ModelState.AddModelError("", "List must contain at least one item");
            return View(model);
        }

        if (model.Any(dc => dc.Count < 1))
        {
            ModelState.AddModelError("", "List must contain items with count more than 0");
            return View(model);
        }

        var updatedCarModel =
            await Client.PatchAsync<IEnumerable<DetailCount>, CarModel>($"car_models/{id}/set-details", model);
        return RedirectToAction("Info", "CarModel", new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Assembly(int id)
    {
        var assembled = await Client.PostAsync<string, Auto>($"autos/assembly/{id}", string.Empty);
        if (assembled.Details is { }) 
            return RedirectToAction("Table", "CarModel");
        
        return RedirectToAction("Info", "Auto", assembled.Value!);
    }
}