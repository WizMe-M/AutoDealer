namespace AutoDealer.DAL.Database.Entity;

public partial class User
{
    public int IdEmployee { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Deleted { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; } = new List<PurchaseRequest>();
}