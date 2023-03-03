namespace AutoDealer.DAL.Database.Entity;

public enum TestStatus
{
    NotChecked,
    Certified,
    Defective
}

public partial class TestAuto
{
    public int IdTest { get; set; }

    public int IdAuto { get; set; }

    public DateOnly? CertificationDate { get; set; }
    
    public TestStatus Status { get; set; }

    public virtual Auto Auto { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;
}