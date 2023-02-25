namespace AutoDealer.API.Abstractions;

public abstract class ApiController : ControllerBase
{
    protected readonly AutoDealerContext Context;

    protected ApiController(AutoDealerContext context)
    {
        Context = context;
    }
}