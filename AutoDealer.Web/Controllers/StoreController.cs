﻿namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[Route("store")]
public class StoreController : MvcController
{
    public StoreController(HttpClient client, IOptions<JsonSerializerOptions> options) : base(client, options)
    {
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details(DetailSort sort = DetailSort.IdAsc)
    {
        ViewBag.IdSort = sort is DetailSort.IdAsc ? DetailSort.IdDesc : DetailSort.IdAsc;
        ViewBag.CostSort = sort is DetailSort.CostAsc ? DetailSort.CostDesc : DetailSort.CostAsc;

        var result = await GetFromApiAsync<Detail[]>($"store/details?sort={sort}");
        if (result.StatusCode is HttpStatusCode.Unauthorized)
            return RedirectToAction("Login", "Auth");
        return View(result.Value ?? ArraySegment<Detail>.Empty);

    }
}