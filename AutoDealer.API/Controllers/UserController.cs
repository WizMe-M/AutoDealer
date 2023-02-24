namespace AutoDealer.API.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly CrudRepositoryBase<User> _repository;
    private readonly HashService _hashService;

    public UserController(CrudRepositoryBase<User> repository, HashService hashService)
    {
        _repository = repository;
        _hashService = hashService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var user = _repository.Get(id);
        return user is { } ? Ok(user) : NotFound();
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    public IActionResult CreateUser([FromBody] NewUser newUser)
    {
        if (newUser is not { Email: { }, Password: { } }) return NoContent();

        var hashedPassword = _hashService.HashPassword(newUser.Password);
        newUser = new NewUser(newUser.EmployeeId, newUser.Email, hashedPassword);
        var user = newUser.Construct();

        if (_repository.Get(user.IdEmployee) is { }) return BadRequest("User with this id already exists");

        var created = _repository.Create(user);
        return Ok(created);
    }

    [HttpPatch("{id:int}/change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    public IActionResult ChangePassword(int id, [FromBody] string password)
    {
        var user = _repository.Get(id);
        if (user is null) return NotFound();

        var hash = _hashService.HashPassword(password);
        user.PasswordHash = hash;
        _repository.Update(user);

        return Ok("User's password was changed");
    }

    [HttpPatch("{id:int}/restore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult RestoreUser(int id)
    {
        var user = _repository.Get(id);
        if (user is null) return NotFound();

        if (!user.Deleted) return BadRequest("User IS NOT deleted");
        user.Deleted = false;

        _repository.Update(user);
        return Ok("User was updated");
    }

    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteUser(int id)
    {
        var user = _repository.Get(id);
        if (user is null) return NotFound();

        if (user.Deleted) return BadRequest("User IS ALREADY deleted");

        _repository.Delete(user);
        return Ok("User was deleted");
    }
}