namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
public class AdminController : MvcController
{
    private readonly PostgresDumpService _dumpService;

    public AdminController(ApiClient client, PostgresDumpService dumpService) : base(client)
    {
        _dumpService = dumpService;
    }

    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var result = await Client.GetAsync<User[]>("users");
        return View(result.Value ?? Array.Empty<User>());
    }

    [HttpGet]
    public IActionResult Backup()
    {
        _dumpService.BackupDatabase("C:/backups/");
        return RedirectToAction("Users", "Admin");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await Client.DeleteAsync<User>($"users/{id}/delete");
        return RedirectToAction("Users", "Admin");
    }

    [HttpGet]
    public async Task<IActionResult> Restore(int id)
    {
        var deleted = await Client.PatchAsync<string, User>($"users/{id}/restore", string.Empty);
        return RedirectToAction("Users", "Admin");
    }
}