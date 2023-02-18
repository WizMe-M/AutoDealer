namespace AutoDealer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserRepository _repository;

    public UserController(UserRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IEnumerable<User> GetAll()
    {
        return _repository.GetAll();
    }

    [HttpGet("[controller]/{id:int}")]
    public User? GetById(int id)
    {
        return _repository.GetById(id);
    }

    [HttpPost("/{employeeId:int}/create")]
    public void CreateUser(int employeeId, string login, string passwordHash)
    {
        _repository.CreateUser(employeeId, login, passwordHash);
    }

    [HttpPatch("/{id:int}/change-password")]
    public void ChangePassword(int id, string passwordHash)
    {
        _repository.EditUser(id, passwordHash);
    }

    [HttpPatch("/{id:int}/restore")]
    public void RestoreUser(int id)
    {
        _repository.EditUser(id, deletedStatus: false);
    }

    [HttpDelete("/{id:int}/delete")]
    public void DeleteUser(int id)
    {
        _repository.DeleteUserById(id);
    }
}