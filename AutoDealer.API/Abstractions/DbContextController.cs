namespace AutoDealer.API.Abstractions;

public abstract class DbContextController : ControllerBase
{
    protected readonly AutoDealerContext Context;

    protected DbContextController(AutoDealerContext context)
    {
        Context = context;
    }
}