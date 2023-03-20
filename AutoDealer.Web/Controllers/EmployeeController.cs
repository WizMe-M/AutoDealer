namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[Route("employee")]
public class EmployeeController : MvcController
{
    public EmployeeController(HttpClient client, IOptions<JsonSerializerOptions> options) : base(client, options)
    {
    }

    [HttpGet("table")]
    public async Task<IActionResult> Table(Post? filter)
    {
        var data = await GetFromApiAsync<Employee[]>($"employees?filter={filter}");
        if (data.StatusCode is HttpStatusCode.Unauthorized)
            return RedirectToAction("Login", "Auth");
        return View(data.Value ?? ArraySegment<Employee>.Empty);
    }
}