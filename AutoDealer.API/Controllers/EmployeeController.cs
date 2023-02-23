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
    public IActionResult CreateEmployee([FromBody] NewEmployee newEmployee)
    {
        if (newEmployee is not { FirstName: { }, LastName: { }, PassportNumber: { }, PassportSeries: { }, Post: { } })
            return NoContent();

        // TODO: add validation
        var employee = newEmployee.Construct();
        var foundWithPassport = _repository.Get(emp =>
            emp.PassportNumber == employee.PassportNumber && emp.PassportSeries == employee.PassportSeries);
        
        if (foundWithPassport is { }) return BadRequest("There is already employee with set passport data");

        var created = _repository.Create(employee);
        return Ok(created);
    }
}