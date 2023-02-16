namespace AutoDealer.DAL.Database.Entity;

public partial class Test
{
    public int IdTest { get; set; }

    public int? IdEmployee { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual Employee? IdEmployeeNavigation { get; set; }

    public virtual ICollection<TestAuto> TestAutos { get; } = new List<TestAuto>();
}