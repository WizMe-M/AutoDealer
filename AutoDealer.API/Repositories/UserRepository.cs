namespace AutoDealer.API.Repositories;

public class UserRepository : RepositoryBase
{
    public UserRepository(AutoDealerContext context) : base(context)
    {
    }

    public void CreateUser(Employee employee, string login, string passwordHash)
    {
        var user = new User()
        {
            Employee = employee,
            Login = login,
            PasswordHash = passwordHash
        };
        Context.Users.Add(user);
        Context.SaveChanges();
    }

    public void EditUser(int id, string passwordHash)
    {
        var user = Context.Users.SingleOrDefault(user => user.IdEmployee == id);
        if (user is null) return;

        user.PasswordHash = passwordHash;
        Context.Users.Update(user);
        Context.SaveChanges();
    }

    public void DeleteUserById(int id)
    {
        var user = new User { IdEmployee = id };
        Context.Users.Remove(user);
        Context.SaveChanges();
    }
}