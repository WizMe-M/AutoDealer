namespace AutoDealer.DAL.Database.Entity;

public partial class User
{
    public int IdEmployee { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool Deleted { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual IEnumerable<PurchaseRequest> PurchaseRequests { get; } = new List<PurchaseRequest>();
}