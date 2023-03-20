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
        var result = await GetFromApiAsync<User[]>("users");
        if (result.StatusCode is HttpStatusCode.Unauthorized)
            return RedirectToAction("Login", "Auth");
        return View(result.Value ?? ArraySegment<User>.Empty);
    }
}