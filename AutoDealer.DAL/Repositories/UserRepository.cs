namespace AutoDealer.DAL.Repositories;

public class UserRepository : CrudRepositoryBase<User>
{
    public UserRepository(AutoDealerContext context) : base(context)
    {
    }

    public override IEnumerable<User> Get()
    {
        return Context.Users.ToArray();
    }

    public override User? Get(int id)
    {
        return Context.Users.SingleOrDefault(user => user.IdEmployee == id);
    }

    public override void Create(User entity)
    {
        Context.Users.Add(entity);
        Context.SaveChanges();
    }

    public override void Update(User entity)
    {
        Context.Users.Update(entity);
        Context.SaveChanges();
    }

    public override void Delete(User entity)
    {
        entity.Deleted = true;
        Update(entity);
    }

    public override void Delete(int id)
    {
        var user = Context.Users.SingleOrDefault(user => user.IdEmployee == id);
        if (user is null) return;
        Delete(user);
    }
}