namespace AutoDealer.DAL.Repositories;

public class UserRepository : CrudRepositoryBase<User>
{
    public UserRepository(AutoDealerContext context) : base(context)
    {
    }

    public void CreateUser(int employeeId, string login, string passwordHash)
    {
        var user = new User
        {
            IdEmployee = employeeId,
            Login = login,
            PasswordHash = passwordHash
        };
        Context.Users.Add(user);
        Context.SaveChanges();
    }
    
    public IEnumerable<User> GetAll()
    {
        return Context.Users.ToArray();
    }

    public User? GetById(int id)
    {
        return Context.Users.SingleOrDefault(user => user.IdEmployee == id);
    }

    public void EditUser(int id, string passwordHash)
    {
        var user = Context.Users.SingleOrDefault(user => user.IdEmployee == id);
        if (user is null) return;

        user.PasswordHash = passwordHash;
        Context.Users.Update(user);
        Context.SaveChanges();
    }

    public void EditUser(int id, bool deletedStatus)
    {
        var user = Context.Users.SingleOrDefault(user => user.IdEmployee == id);
        if (user is null) return;

        user.Deleted = deletedStatus;
        Context.Users.Update(user);
        Context.SaveChanges();
    }

    public void DeleteUserById(int id)
    {
        EditUser(id, deletedStatus: true);
    }
}