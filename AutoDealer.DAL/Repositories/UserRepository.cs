﻿namespace AutoDealer.DAL.Repositories;

public class UserRepository : CrudRepositoryBase<User>
{
    public UserRepository(AutoDealerContext context) : base(context)
    {
    }

    public override IEnumerable<User> GetAll()
    {
        return Context.Users
            .Include(user => user.Employee)
            .ToArray();
    }

    public override IEnumerable<User> GetAll(Func<User, bool> predicate)
    {
        return Context.Users
            .Include(user => user.Employee)
            .Where(predicate).ToArray();
    }

    public override User? Get(int id)
    {
        return Context.Users
            .Include(user => user.Employee)
            .FirstOrDefault(user => user.IdEmployee == id);
    }

    public override User? Get(Func<User, bool> predicate)
    {
        return Context.Users
            .Include(user => user.Employee)
            .FirstOrDefault(predicate);
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