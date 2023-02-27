namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[ApiController]
[Route("employees")]
public class EmployeeController : DbContextController
{
    public EmployeeController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var employees = Context.Employees.ToArray();
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var employee = Find(id);
        return employee is { } ? Ok(employee) : NotFound("Employee with such ID doesn't exist");
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateEmployee([FromBody] NewEmployee newEmployee)
    {
        // TODO: add validation
        var employee = newEmployee.Construct();
        var foundWithPassport = Context.Employees.FirstOrDefault(emp =>
            emp.PassportNumber == employee.PassportNumber &&
            emp.PassportSeries == employee.PassportSeries) is { };

        if (foundWithPassport) return BadRequest("There is already employee with set passport data");

        Context.Employees.Add(employee);
        Context.SaveChanges();

        return Ok(employee);
    }

    [HttpPatch("{id:int}/update-passport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdatePassport(int id, [FromBody] Passport passport)
    {
        var employee = Find(id);
        if (employee is null) return NotFound("Employee with such ID doesn't exist");

        var foundWithPassport = Context.Employees.FirstOrDefault(emp =>
            emp.Id != id
            && emp.PassportSeries == passport.Series
            && emp.PassportNumber == passport.Number) is { };
        if (foundWithPassport)
            return BadRequest("There is already another employee with set passport data");

        employee.PassportSeries = passport.Series;
        employee.PassportNumber = passport.Number;
        Context.Employees.Update(employee);
        Context.SaveChanges();

        return Ok("Employee's passport was updated");
    }

    [HttpPatch("{id:int}/update-full-name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateFullName(int id, [FromBody] FullName fullName)
    {
        var employee = Find(id);
        if (employee is null) return NotFound("Employee with such ID doesn't exist");

        employee.FirstName = fullName.FirstName;
        employee.LastName = fullName.LastName;
        employee.MiddleName = fullName.MiddleName;
        Context.Employees.Update(employee);
        Context.SaveChanges();

        return Ok("Employee's full name was updated");
    }

    [HttpPatch("{id:int}/promote-to-post")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PromoteToPost(int id, [FromBody] Post post)
    {
        var employee = Find(id);
        if (employee is null) return NotFound("Employee with such ID doesn't exist");

        employee.Post = post;
        Context.Employees.Update(employee);
        Context.SaveChanges();

        return Ok("Employee was promoted to new post");
    }

    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var employee = Find(id);
        if (employee is null) return NotFound("Employee with such ID doesn't exist");

        Context.Employees.Remove(employee);
        Context.SaveChanges();

        return Ok("Employee was deleted");
    }

    private Employee? Find(int id) => Context.Employees.FirstOrDefault(e => e.Id == id);
}