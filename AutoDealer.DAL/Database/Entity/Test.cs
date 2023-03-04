namespace AutoDealer.DAL.Database.Entity;

public partial class Test
{
    public int Id { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<TestAuto> TestAutos { get; } = new List<TestAuto>();
}