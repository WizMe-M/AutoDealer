namespace AutoDealer.DAL.Database.Entity;

public enum AutoStatus
{
    Assembled,
    Testing,
    Selling,
    Sold
}

public partial class Auto
{
    public int Id { get; set; }

    [JsonIgnore] public int IdCarModel { get; set; }

    public DateOnly? AssemblyDate { get; set; }

    public decimal? Cost { get; set; }

    public AutoStatus Status { get; set; }

    public virtual IEnumerable<Detail> Details { get; } = new List<Detail>();

    public virtual CarModel CarModel { get; set; } = null!;

    [JsonIgnore] public virtual IEnumerable<Sale> Sales { get; } = new List<Sale>();

    [JsonIgnore] public virtual IEnumerable<TestAuto> TestAutos { get; } = new List<TestAuto>();
}