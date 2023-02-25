﻿namespace AutoDealer.DAL.Database.Entity;

public partial class Model
{
    public int Id { get; set; }

    [JsonIgnore]
    public int IdLine { get; set; }

    public string Name { get; set; } = null!;

    public virtual Line Line { get; set; } = null!;

    [JsonIgnore]
    public virtual IEnumerable<Trim> Trims { get; } = new List<Trim>();
}