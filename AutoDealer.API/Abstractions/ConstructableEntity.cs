namespace AutoDealer.API.Abstractions;

public abstract record ConstructableEntity<TEntity>
{
    public abstract TEntity Construct();
}