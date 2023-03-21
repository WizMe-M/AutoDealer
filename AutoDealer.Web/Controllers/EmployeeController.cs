using Microsoft.IdentityModel.Tokens;

namespace AutoDealer.Web.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[Route("employee")]
public class EmployeeController : MvcController
{
    public EmployeeController(ApiClient client) : base(client)
    {
    }

    [HttpGet("table")]
    public async Task<IActionResult> Table(Post? filter)
    {
        var data = await Client.GetAsync<Employee[]>($"employees?filter={filter}");
        return View(data.Value ?? ArraySegment<Employee>.Empty);
    }

    [HttpGet("create")]
    public IActionResult Create() => View();

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateEmployeeViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var data = new EmployeeData(
            new FullName(vm.FirstName, vm.LastName, vm.MiddleName),
            new Passport(vm.PassportSeries, vm.PassportNumber),
            vm.Post);
        var result = await Client.PostAsync<EmployeeData, Employee>("employees/create", data);
        if (result.Details is null) return RedirectToAction("Table");
        
        ModelState.AddModelError("", result.Details!);
        return View();
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Client.DeleteAsync<Employee>($"employees/{id}/delete");
        return RedirectToAction("Table");
    }
}