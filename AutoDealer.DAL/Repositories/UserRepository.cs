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

    public override User Create(User entity)
    {
        Context.Users.Add(entity);
        Context.SaveChanges();
        return entity;
    }

    public override void Update(User entity)
    {
        Context.Users.Update(entity);
        Context.SaveChanges();
    }

    public override User? Delete(User? entity)
    {
        if (entity is null) return null;
        entity.Deleted = true;
        Update(entity);
        return entity;
    }

    public override User? Delete(int id)
    {
        var user = Context.Users.SingleOrDefault(user => user.IdEmployee == id);
        return Delete(user);
    }
}