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

    public override void Delete(User entity)
    {
        entity.Deleted = true;
        Update(entity);
    }
}