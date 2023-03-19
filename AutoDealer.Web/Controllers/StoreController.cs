using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using AutoDealer.DAL.Database.Entity;
using AutoDealer.Utility.Sort;
using AutoDealer.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[Route("store")]
public class StoreController : MvcController
{
    public StoreController(HttpClient client) : base(client)
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
            var details = node["data"].Deserialize<Detail[]>() ?? ArraySegment<Detail>.Empty;
            return View(details);
        }

        if (responseMessage.StatusCode is HttpStatusCode.Unauthorized)
            return RedirectToAction("Login", "Auth");

        var response = await responseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        ModelState.AddModelError("resp-error", response!.Detail!);
        return View();
    }
}