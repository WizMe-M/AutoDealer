namespace AutoDealer.Web.Controllers;

public class MarginController : MvcController
{
    public MarginController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Table(MarginSort sort = MarginSort.IdAsc)
    {
        ViewBag.IdSort = sort is MarginSort.IdAsc ? MarginSort.IdDesc : MarginSort.IdAsc;
        ViewBag.ValueSort = sort is MarginSort.ValueAsc ? MarginSort.ValueDesc : MarginSort.ValueAsc;
        ViewBag.StartDateSort = sort is MarginSort.StartDateAsc ? MarginSort.StartDateDesc : MarginSort.StartDateAsc;
        
        var margins = await Client.GetAsync<Margin[]>($"margins?sort={sort}");
        var carModels = await Client.GetAsync<CarModel[]>("car_models");
        
        ViewBag.Margins = margins.Value ?? Array.Empty<Margin>();
        ViewBag.CarModels = carModels.Value ?? Array.Empty<CarModel>();
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(MarginData data)
    {
        if (!ModelState.IsValid)
        {
            var margins = await Client.GetAsync<Margin[]>("margins");
            var carModels = await Client.GetAsync<CarModel[]>("car_models");
        
            ViewBag.Margins = margins.Value ?? Array.Empty<Margin>();
            ViewBag.CarModels = carModels.Value ?? Array.Empty<CarModel>();
            return View("Table", data);
        }
        var margin = await Client.PostAsync<MarginData, Margin>("margins/set-margin", data);
        
        if (margin.Details is null) return RedirectToAction("Table", "Margin");
        
        ModelState.AddModelError("add", margin.Details);
        return View("Table", data);

    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var margin = await Client.DeleteAsync<Margin>($"margins/delete/{id}");
        if (margin.Details is null) return RedirectToAction("Table", "Margin");
        
        ModelState.AddModelError("delete", margin.Details);
        return View("Table");
    }
}