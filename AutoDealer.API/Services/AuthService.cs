namespace AutoDealer.API.Services;

public class AuthService
{
    private readonly AutoDealerContext _context;

    public AuthService(AutoDealerContext context)
    {
        _context = context;
    }

    public User? Authorize(string login, string passwordHash, Post post)
    {
        return _context.Users
            .Include(user => user.Employee)
            .SingleOrDefault(user => user.Login == login
                                     && user.PasswordHash == passwordHash
                                     && user.Employee.Post == post);
    }
}