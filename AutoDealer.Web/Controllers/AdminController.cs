namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
public class AdminController : MvcController
{
    public AdminController(ApiClient client) : base(client)
    {
    }

    public async Task<IActionResult> Users()
    {
        var result = await Client.GetAsync<User[]>("users");
        return View(result.Value ?? Array.Empty<User>());
    }
}