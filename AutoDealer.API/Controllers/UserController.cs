namespace AutoDealer.API.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly CrudRepositoryBase<User> _repository;

    public UserController(CrudRepositoryBase<User> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    public IEnumerable<User> GetAll()
    {
        return _repository.Get();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var user = _repository.Get(id);
        return user is { } ? Ok(user) : NotFound();
    }

    [HttpPost("{id:int}/create")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    public IActionResult CreateUser([FromRoute] int id, [FromBody] NewUser newUser)
    {
        if (newUser is not { Login: { }, Password: { } }) return NoContent();

        var user = new User
        {
            IdEmployee = id,
            Login = newUser.Login,
            PasswordHash = newUser.Password
        };

        try
        {
            var created = _repository.Create(user);
            return Ok(created);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPatch("{id:int}/change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    public IActionResult ChangePassword(int id, [FromBody] string passwordHash)
    {
        var user = _repository.Get(id);
        if (user is null) return NotFound();
        user.PasswordHash = passwordHash;
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