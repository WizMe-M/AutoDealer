namespace AutoDealer.API.Controllers;

[ApiController]
[Route("employees")]
public class EmployeeController : ControllerBase
{
    private readonly CrudRepositoryBase<Employee> _repository;

    public EmployeeController(CrudRepositoryBase<Employee> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var employee = _repository.Get(id);
        return employee is { } ? Ok(employee) : NotFound();
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json")]
    public IActionResult CreateEmployee([FromBody] NewEmployee newEmployee)
    {
        if (newEmployee is not { FirstName: { }, LastName: { }, PassportNumber: { }, PassportSeries: { }, Post: { } })
            return NoContent();

        // TODO: add validation
        var employee = newEmployee.Construct();
        var foundWithPassport = _repository.Get(emp =>
            emp.PassportNumber == employee.PassportNumber &&
            emp.PassportSeries == employee.PassportSeries) is { };

        if (foundWithPassport) return BadRequest("There is already employee with set passport data");

        var created = _repository.Create(employee);
        return Ok(created);
    }

    [HttpPatch("{id:int}/update-passport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    public IActionResult UpdatePassport(int id, [FromBody] Passport passport)
    {
        if (passport is not { Series: { }, Number: { } }) return NoContent();

        var employee = _repository.Get(id);
        if (employee is null) return NotFound();

        var foundWithPassport = _repository.Get(emp =>
            emp.Id != id
            && emp.PassportSeries == passport.Series
            && emp.PassportNumber == passport.Number) is { };

        if (foundWithPassport)
            return BadRequest("There is already another employee with set passport data");

        employee.PassportSeries = passport.Series;
        employee.PassportNumber = passport.Number;
        _repository.Update(employee);

        return Ok("Employee's passport was updated");
    }

    [HttpPatch("{id:int}/update-full-name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    public IActionResult UpdateFullName(int id, [FromBody] FullName fullName)
    {
        if (fullName is not { FirstName: { }, LastName: { } }) return NoContent();

        var employee = _repository.Get(id);
        if (employee is null) return NotFound();

        employee.FirstName = fullName.FirstName;
        employee.LastName = fullName.LastName;
        employee.MiddleName = fullName.MiddleName;
        _repository.Update(employee);

        return Ok("Employee's full name was updated");
    }

    [HttpPatch("{id:int}/promote-to-post")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PromoteToPost(int id, [FromBody] Post post)
    {
        var employee = _repository.Get(id);
        if (employee is null) return NotFound();

        employee.Post = post;
        _repository.Update(employee);

        return Ok("Employee was promoted to new post");
    }

    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var employee = _repository.Get(id);
        if (employee is null) return NotFound();
        
        _repository.Delete(employee);
        
        return Ok("Employee was deleted");
    }
}