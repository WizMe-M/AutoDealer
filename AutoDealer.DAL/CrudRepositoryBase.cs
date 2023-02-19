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
    public abstract TEntity Create(TEntity entity);
    public abstract void Update(TEntity entity);
    public abstract TEntity? Delete(TEntity? entity);
    public abstract TEntity? Delete(int id);
}