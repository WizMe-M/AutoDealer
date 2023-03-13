namespace AutoDealer.API.Controllers.API;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[ApiController]
[Route("employees")]
public class EmployeeController : DbContextController<Employee>
{
    public EmployeeController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll(Post? filter)
    {
        var employees = filter is { }
            ? Context.Employees.Where(employee => employee.Post == filter).ToArray()
            : Context.Employees.ToArray();

        return Ok("All employees listed", employees);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var employee = Find(id);
        return employee is { }
            ? Ok("Employee found", employee)
            : Problem(detail:"Employee with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);
    }

    [HttpPost("create")]
    public IActionResult CreateEmployee([FromBody] EmployeeData data)
    {
        var employee = new Employee
        {
            FirstName = data.FullName.FirstName,
            LastName = data.FullName.LastName,
            MiddleName = data.FullName.MiddleName,
            PassportNumber = data.Passport.Number,
            PassportSeries = data.Passport.Series,
            Post = data.Post
        };

        var foundWithPassport = Context.Employees.FirstOrDefault(emp =>
            emp.PassportNumber == employee.PassportNumber &&
            emp.PassportSeries == employee.PassportSeries) is { };

        if (foundWithPassport) return Problem("There is already employee with set passport data");

        Context.Employees.Add(employee);
        Context.SaveChanges();

        return Ok("Employee was successfully created", employee);
    }

    [HttpPatch("{id:int}/update-passport")]
    public IActionResult UpdatePassport(int id, [FromBody] Passport passport)
    {
        var found = Find(id);
        if (found is null) return Problem(detail:"Employee with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        var foundWithPassport = Context.Employees.FirstOrDefault(emp =>
            emp.Id != id
            && emp.PassportSeries == passport.Series
            && emp.PassportNumber == passport.Number) is { };
        if (foundWithPassport)
            return Problem("There is already another employee with set passport data");

        found.PassportSeries = passport.Series;
        found.PassportNumber = passport.Number;
        Context.Employees.Update(found);
        Context.SaveChanges();

        return Ok("Employee's passport was updated", found);
    }

    [HttpPatch("{id:int}/update-full-name")]
    public IActionResult UpdateFullName(int id, [FromBody] FullName fullName)
    {
        var found = Find(id);
        if (found is null) return NotFound("Employee with such ID doesn't exist");

        found.FirstName = fullName.FirstName;
        found.LastName = fullName.LastName;
        found.MiddleName = fullName.MiddleName;
        Context.Employees.Update(found);
        Context.SaveChanges();

        return Ok("Employee's full name was updated", found);
    }

    [HttpPatch("{id:int}/promote-to-post")]
    public IActionResult PromoteToPost(int id, [FromBody] Post post)
    {
        var found = Find(id);
        if (found is null) return Problem(detail:"Employee with such ID doesn't exist", statusCode:StatusCodes.Status404NotFound);

        found.Post = post;
        Context.Employees.Update(found);
        Context.SaveChanges();

        return Ok("Employee was promoted to new post", found);
    }

    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var employee = Find(id);
        if (employee is null) return Problem(detail:"Employee with such ID doesn't exist", statusCode:StatusCodes.Status404NotFound);

        Context.Employees.Remove(employee);
        Context.SaveChanges();

        return Ok("Employee was deleted", employee);
    }

    private Employee? Find(int id) => Context.Employees.FirstOrDefault(e => e.Id == id);

    protected override Task LoadReferencesAsync(Employee entity) => throw new NotSupportedException();
}