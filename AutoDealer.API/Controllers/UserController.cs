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
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var users = Context.Users.Include(user => user.Employee).ToArray();
        return Ok("All users listed", users);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var user = Find(id);
        return user is { }
            ? Ok("User found", user)
            : NotFound("User with such ID doesn't exist");
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] string password)
    {
        var found = Find(id);
        if (found is null) return NotFound("User with such id doesn't exist");

        var hash = _hashService.HashPassword(password);
        found.PasswordHash = hash;
        Context.Users.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("User's password was changed", found);
    }

    [HttpDelete("{id:int}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound("User with such id doesn't exist");

        if (found.Deleted) return BadRequest("User is already deleted");
        found.Deleted = true;
        Context.Users.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("User was deleted", found);
    }

    [HttpPatch("{id:int}/restore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Restore(int id)
    {
        var found = Find(id);
        if (found is null) return NotFound("User with such id doesn't exist");

        if (!found.Deleted) return BadRequest("User is not deleted");
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