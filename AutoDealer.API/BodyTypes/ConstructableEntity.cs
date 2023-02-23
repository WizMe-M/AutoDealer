﻿namespace AutoDealer.API.BodyTypes;

public abstract record ConstructableEntity<TEntity>
{
    public abstract TEntity Construct();
}