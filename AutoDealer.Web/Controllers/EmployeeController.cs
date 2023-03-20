using Microsoft.IdentityModel.Tokens;

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
        var data = await GetApiAsync<Employee[]>($"employees?filter={filter}");
        return View(data.Value ?? ArraySegment<Employee>.Empty);
    }

    [HttpGet("create")]
    public IActionResult Create() => View();

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateEmployeeViewModel vm)
    {
        if (!ModelState.IsValid) return View();
        var data = new EmployeeData(
            new FullName(vm.FirstName, vm.LastName, vm.MiddleName),
            new Passport(vm.PassportSeries, vm.PassportNumber),
            vm.Post);
        await PostApiAsync<EmployeeData, Employee>("employees/create", data);
        return RedirectToAction("Table");
    }
}