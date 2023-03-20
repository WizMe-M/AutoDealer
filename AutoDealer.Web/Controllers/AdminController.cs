namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[Route("admin")]
public class AdminController : MvcController
{
    public AdminController(HttpClient client, IOptions<JsonSerializerOptions> options) : base(client, options)
    {
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        var responseMessage = await ApiClient.GetAsync("users");
        if (responseMessage.IsSuccessStatusCode)
        {
            var result = await responseMessage.Content.ReadFromJsonAsync<dynamic>();
            var node = JsonSerializer.Deserialize<JsonNode>((string)result!.ToString())!;
            var users = node["data"].Deserialize<User[]>(JsonSerializerOptions);
            return View(users ?? Array.Empty<User>());
        }

        if (responseMessage.StatusCode is HttpStatusCode.Unauthorized)
            return RedirectToAction("Login", "Auth");

        return View(ArraySegment<User>.Empty);
    }
}