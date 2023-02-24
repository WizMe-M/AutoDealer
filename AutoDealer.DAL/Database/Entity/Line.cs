﻿namespace AutoDealer.DAL.Database.Entity;

public partial class Line
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual IEnumerable<Model> Models { get; } = new List<Model>();
}