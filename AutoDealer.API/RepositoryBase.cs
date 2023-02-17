namespace AutoDealer.API;

public abstract class RepositoryBase
{
    protected readonly AutoDealerContext Context;

    protected RepositoryBase(AutoDealerContext context)
    {
        Context = context;
    }
}