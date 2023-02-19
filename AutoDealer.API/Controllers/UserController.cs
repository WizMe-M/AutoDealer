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
    public IEnumerable<User> GetAll()
    {
        return _repository.Get();
    }

    [HttpGet("{id:int}")]
    public User? GetById(int id)
    {
        return _repository.Get(id);
    }

    [HttpPost("{id:int}/create")]
    public void CreateUser([FromRoute] int id, [FromBody] NewUser newUser)
    {
        var user = new User
        {
            IdEmployee = id,
            Login = newUser.Login,
            PasswordHash = newUser.Password
        };
        _repository.Create(user);
    }

    [HttpPatch("{id:int}/change-password")]
    public void ChangePassword(int id, [FromBody] string passwordHash)
    {
        var user = _repository.Get(id);
        if (user is null) return;
        user.PasswordHash = passwordHash;
        _repository.Update(user);
    }

    [HttpPatch("{id:int}/restore")]
    public void RestoreUser(int id)
    {
        var user = _repository.Get(id);
        if (user is null) return;
        user.Deleted = false;
        _repository.Update(user);
    }

    [HttpDelete("{id:int}/delete")]
    public void DeleteUser(int id)
    {
        _repository.Delete( id);
    }
}