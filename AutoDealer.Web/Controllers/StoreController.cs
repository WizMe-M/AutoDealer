namespace AutoDealer.Web.Controllers;

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

        var responseMessage = await ApiClient.GetAsync($"store/details?sort={sort}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var result = await responseMessage.Content.ReadFromJsonAsync<dynamic>();
            var node = JsonSerializer.Deserialize<JsonNode>((string)result!.ToString())!;
            var details = node["data"].Deserialize<Detail[]>(JsonSerializerOptions);
            return View(details ?? ArraySegment<Detail>.Empty);
        }

        if (responseMessage.StatusCode is HttpStatusCode.Unauthorized)
            return RedirectToAction("Login", "Auth");

        var response = await responseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        ModelState.AddModelError("resp-error", response!.Detail!);
        return View();
    }
}