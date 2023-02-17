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

    [HttpPatch("/{id:int}/change-password")]
    public void ChangePassword(int id, string passwordHash)
    {
        _repository.EditUser(id, passwordHash);
    }

    [HttpDelete("/{id:int}/delete")]
    public void DeleteUser(int id)
    {
        _repository.DeleteUserById(id);
    }
}