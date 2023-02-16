namespace AutoDealer.DAL.Entity;

public partial class TestAuto
{
    public int IdTest { get; set; }

    public int IdAuto { get; set; }

    public DateOnly CertificationDate { get; set; }
    
    public TestStatus Status { get; set; }

    public virtual Auto IdAutoNavigation { get; set; } = null!;

    public virtual Test IdTestNavigation { get; set; } = null!;
}