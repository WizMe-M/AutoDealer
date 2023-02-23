namespace AutoDealer.DAL.Repositories;

public class EmployeeRepository : CrudRepositoryBase<Employee>
{
    public EmployeeRepository(AutoDealerContext context) : base(context)
    {
    }

    public override IEnumerable<Employee> GetAll()
    {
        return Context.Employees.ToArray();
    }

    public override IEnumerable<Employee> GetAll(Func<Employee, bool> predicate)
    {
        return Context.Employees.Where(predicate).ToArray();
    }

    public override Employee? Get(int id)
    {
        return Context.Employees.FirstOrDefault(employee => employee.Id == id);
    }

    public override Employee? Get(Func<Employee, bool> predicate)
    {
        return Context.Employees.FirstOrDefault(predicate);
    }

    public override Employee Create(Employee entity)
    {
        Context.Employees.Add(entity);
        Context.SaveChanges();
        return entity;
    }

    public override void Update(Employee entity)
    {
        Context.Employees.Update(entity);
        Context.SaveChanges();
    }

    public override void Delete(Employee entity)
    {
        Context.Employees.Remove(entity);
        Context.SaveChanges();
    }
}