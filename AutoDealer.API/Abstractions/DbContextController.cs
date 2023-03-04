namespace AutoDealer.API.Abstractions;

public abstract class DbContextController<T> : ControllerBase
{
    protected readonly AutoDealerContext Context;

    protected DbContextController(AutoDealerContext context)
    {
        Context = context;
    }

    protected OkObjectResult Ok(string message, object? data = null)
    {
        var result = new { message, data };
        return base.Ok(result);
    }

    protected abstract Task LoadReferencesAsync(T entity);
}