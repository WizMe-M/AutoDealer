namespace AutoDealer.DAL;

public abstract class CrudRepositoryBase<TEntity>
{
    protected readonly AutoDealerContext Context;

    protected CrudRepositoryBase(AutoDealerContext context)
    {
        Context = context;
    }
}