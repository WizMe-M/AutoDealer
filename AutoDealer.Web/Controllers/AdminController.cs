namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[Route("admin")]
public class AdminController : MvcController
{
    public AdminController(ApiClient client) : base(client)
    {
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        var result = await Client.GetAsync<User[]>("users");
        return View(result.Value ?? ArraySegment<User>.Empty);
    }
}