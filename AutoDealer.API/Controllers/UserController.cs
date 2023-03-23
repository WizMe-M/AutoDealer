namespace AutoDealer.API.Controllers;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[ApiController]
[Route("users")]
public class UserController : DbContextController<User>
{
    private readonly HashService _hashService;

    public UserController(AutoDealerContext context, HashService hashService) : base(context)
    {
        _hashService = hashService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = Context.Users.Include(user => user.Employee).ToArray();
        return Ok("All users listed", users);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var user = Find(id);
        return user is { }
            ? Ok("User found", user)
            : Problem(detail: "User with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] UserData userData)
    {
        var hashedPassword = _hashService.HashPassword(userData.Password);
        var created = new User
        {
            IdEmployee = userData.EmployeeId,
            Email = userData.Email,
            PasswordHash = hashedPassword
        };

        Context.Users.Add(created);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(created);

        return Ok("User successfully created", created);
    }

    [HttpPatch("{id:int}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] string password)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "User with such id doesn't exist", statusCode: StatusCodes.Status404NotFound);

        var hash = _hashService.HashPassword(password);
        found.PasswordHash = hash;
        Context.Users.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("User's password was changed", found);
    }

    [HttpDelete("{id:int}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var found = Find(id);
        if (found is null)
            return Problem(detail: "User with such id doesn't exist", statusCode: StatusCodes.Status404NotFound);

        if (found.Deleted)
            return Problem(detail: "User is already deleted", statusCode: StatusCodes.Status400BadRequest);
        found.Deleted = true;
        Context.Users.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("User was deleted", found);
    }

    [HttpPatch("{id:int}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var found = Find(id);
        switch (found)
        {
            case null:
                return Problem(detail: "User with such id doesn't exist", statusCode: StatusCodes.Status404NotFound);
            case { Deleted: false }:
                return Problem(detail: "User is not deleted", statusCode: StatusCodes.Status400BadRequest);
        }

        found.Deleted = false;
        Context.Users.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("User was restored", found);
    }

    private User? Find(int id)
    {
        return Context.Users
            .Include(user => user.Employee)
            .FirstOrDefault(user => user.IdEmployee == id);
    }

    protected override async Task LoadReferencesAsync(User entity)
    {
        await Context.Users.Entry(entity).Reference(user => user.Employee).LoadAsync();
    }
}