namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[Route("employee")]
public class EmployeeController : MvcController
{
    public EmployeeController(HttpClient client, IOptions<JsonSerializerOptions> options) : base(client, options)
    {
    }

    [HttpGet("table")]
    public async Task<IActionResult> Table()
    {
        var data = await GetFromApiAsync<Employee[]>("employees");
        if (data.StatusCode is HttpStatusCode.Unauthorized)
            return RedirectToAction("Logout", "Auth");
        return View(data.Value ?? ArraySegment<Employee>.Empty);
    }
}