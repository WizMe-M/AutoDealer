using System.ComponentModel.DataAnnotations;

namespace AutoDealer.DAL.Database.Entity;

public enum Post
{
    [Display(Name = "Database administrator")]
    DatabaseAdmin,

    [Display(Name = "Head of assembly workshop")]
    AssemblyChief,

    [Display(Name = "Purchasing specialist")]
    PurchaseSpecialist,

    [Display(Name = "Warehouse employee")] Storekeeper,

    [Display(Name = "Sales specialist")] Seller,

    [Display(Name = "Certification specialist")]
    Tester
}

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string PassportSeries { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public Post Post { get; set; }

    [JsonIgnore] public virtual IEnumerable<Contract> Contracts { get; } = new List<Contract>();

    [JsonIgnore] public virtual IEnumerable<Sale> Sales { get; } = new List<Sale>();

    [JsonIgnore] public virtual User? User { get; set; }
}