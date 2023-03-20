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
        var result = await GetApiAsync<User[]>("users");
        return View(result.Value ?? ArraySegment<User>.Empty);
    }
}