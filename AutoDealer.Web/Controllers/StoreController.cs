namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.Storekeeper))]
[Route("store")]
public class StoreController : MvcController
{
    public StoreController(ApiClient client) : base(client)
    {
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details(DetailSort sort = DetailSort.IdAsc)
    {
        ViewBag.IdSort = sort is DetailSort.IdAsc ? DetailSort.IdDesc : DetailSort.IdAsc;
        ViewBag.CostSort = sort is DetailSort.CostAsc ? DetailSort.CostDesc : DetailSort.CostAsc;

        var result = await Client.GetAsync<Detail[]>($"store/details?sort={sort}");
        return View(result.Value ?? ArraySegment<Detail>.Empty);
    }
}