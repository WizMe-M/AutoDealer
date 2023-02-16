﻿namespace AutoDealer.DAL.Database.Entity;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string PassportSeries { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public Post Post { get; set; }

    public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();

    public virtual ICollection<Test> Tests { get; } = new List<Test>();

    public virtual User? User { get; set; }
}