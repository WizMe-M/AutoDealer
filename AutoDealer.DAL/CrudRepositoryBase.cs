namespace AutoDealer.DAL;

public abstract class CrudRepositoryBase<TEntity>
{
    protected readonly AutoDealerContext Context;

    protected CrudRepositoryBase(AutoDealerContext context)
    {
        Context = context;
    }

    public abstract IEnumerable<TEntity> Get();
    public abstract TEntity? Get(int id);
    public abstract void Create(TEntity entity);
    public abstract void Update(TEntity entity);
    public abstract void Delete(TEntity entity);
    public abstract void Delete(int id);
}