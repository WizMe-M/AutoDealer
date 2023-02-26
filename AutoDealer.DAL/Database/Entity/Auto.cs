namespace AutoDealer.DAL.Database.Entity;

public enum AutoStatus
{
    InAssembly,
    ReadyToTest,
    InTest,
    ReadyToSale,
    Sold
}

public partial class Auto
{
    public int IdAuto { get; set; }

    public int IdCarModel { get; set; }

    public DateOnly? AssemblyDate { get; set; }

    public decimal? Cost { get; set; }

    public AutoStatus Status { get; set; }

    public virtual ICollection<Detail> Details { get; } = new List<Detail>();

    public virtual CarModel CarModel { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();

    public virtual ICollection<TestAuto> TestAutos { get; } = new List<TestAuto>();

    public virtual ICollection<Work> Works { get; } = new List<Work>();
}