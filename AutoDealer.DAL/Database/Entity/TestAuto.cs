namespace AutoDealer.DAL.Database.Entity;

public enum TestStatus
{
    [Display(Name = "Not checked yet")]
    NotChecked,
    
    [Display(Name = "Checked and certified")]
    Certified,

    [Display(Name = "Detected defect")]
    Defective
}

public partial class TestAuto
{
    public int IdTest { get; set; }

    public int IdAuto { get; set; }

    public DateOnly? CertificationDate { get; set; }

    public TestStatus Status { get; set; }

    public virtual Auto Auto { get; set; } = null!;
    
    [JsonIgnore] public virtual Test Test { get; set; } = null!;
}