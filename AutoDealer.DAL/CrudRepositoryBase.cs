namespace AutoDealer.DAL;

public abstract class CrudRepositoryBase<TEntity>
{
    protected readonly AutoDealerContext Context;

    protected CrudRepositoryBase(AutoDealerContext context)
    {
        Context = context;
    }

    public abstract IEnumerable<TEntity> GetAll();
    public abstract IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate);
    public abstract TEntity? Get(int id);
    public abstract TEntity? Get(Func<TEntity, bool> predicate);
    public abstract TEntity Create(TEntity entity);
    public abstract void Update(TEntity entity);
    public abstract void Delete(TEntity entity);
}