namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
public class EmployeeController : MvcController
{
    public EmployeeController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Table(Post? filter)
    {
        var data = await Client.GetAsync<Employee[]>($"employees?filter={filter}");
        return View(data.Value ?? Array.Empty<Employee>());
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeData data)
    {
        if (!ModelState.IsValid) return View(data);

        var result = await Client.PostAsync<EmployeeData, Employee>("employees/create", data);
        if (result.Details is null) return RedirectToAction("Table");

        ModelState.AddModelError("", result.Details!);
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        await Client.DeleteAsync<Employee>($"employees/{id}/delete");
        return RedirectToAction("Table");
    }
}