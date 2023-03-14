namespace AutoDealer.API.Services;

public class AuthService
{
    private readonly AutoDealerContext _context;

    public AuthService(AutoDealerContext context)
    {
        _context = context;
    }

    public User? Authorize(string email, string passwordHash, Post post)
    {
        return _context.Users
            .Include(user => user.Employee)
            .FirstOrDefault(user =>
                user.Email == email &&
                user.PasswordHash == passwordHash &&
                user.Employee.Post == post &&
                user.Deleted == false);
    }

    public User? GetUser(int id)
    {
        return _context.Users
            .Include(user => user.Employee)
            .FirstOrDefault(user => user.IdEmployee == id);
    }

}