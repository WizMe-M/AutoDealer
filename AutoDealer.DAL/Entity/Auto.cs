namespace AutoDealer.DAL.Entity;

public partial class Auto
{
    public int IdAuto { get; set; }

    public int IdTrim { get; set; }

    public DateOnly? AssemblyDate { get; set; }

    public decimal? Cost { get; set; }

    public AutoStatus Status { get; set; }

    public virtual ICollection<Detail> Details { get; } = new List<Detail>();

    public virtual Trim IdTrimNavigation { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();

    public virtual ICollection<TestAuto> TestAutos { get; } = new List<TestAuto>();

    public virtual ICollection<Work> Works { get; } = new List<Work>();
}