namespace AutoDealer.DAL.Database.Entity;

public partial class Client
{
    public int IdClient { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public DateOnly Birthdate { get; set; }

    public string Birthplace { get; set; } = null!;

    public string PassportSeries { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public string PassportIssuer { get; set; } = null!;

    public string DepartmentCode { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();
}