namespace AutoDealer.API.Controllers;

[ApiController]
[Route("api/users")]
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

    [HttpPost("{employeeId:int}/create")]
    public void CreateUser(int employeeId, string login, string passwordHash)
    {
        var user = new User
        {
            IdEmployee = employeeId,
            Login = login,
            PasswordHash = passwordHash
        };
        _repository.Create(user);
    }

    [HttpPatch("{id:int}/change-password")]
    public void ChangePassword(int id, string passwordHash)
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