namespace AutoDealer.Web.Controllers;

public class DetailSeriesController : MvcController
{
    public DetailSeriesController(ApiClient client) : base(client)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> Table()
    {
        var apiResult = await Client.GetAsync<DetailSeries[]>("detail_series");
        ViewBag.Details = apiResult.Value ?? Array.Empty<DetailSeries>();
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(string code)
    {
        if (!ModelState.IsValid) return View("Table", code);
        var apiResult = await Client.PostAsync<string, DetailSeries>("detail_series/create", code);
        
        if (apiResult.Details is null) 
            return RedirectToAction("Table", "DetailSeries");
        
        ModelState.AddModelError("", apiResult.Details);
        return View(code);
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        await Client.DeleteAsync<DetailSeries>($"detail_series/{id}/delete");
        return RedirectToAction("Table", "DetailSeries");
    }

}