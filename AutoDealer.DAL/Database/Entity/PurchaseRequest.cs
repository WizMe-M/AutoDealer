namespace AutoDealer.DAL.Database.Entity;

public enum RequestStatus
{
    Sent,
    InHandling,
    Closed
}

public partial class PurchaseRequest
{
    public int IdPurchaseRequests { get; set; }

    public int? IdUser { get; set; }

    public DateTime SentDate { get; set; }

    public DateOnly ExpectedSupplyDate { get; set; }

    public RequestStatus Status { get; set; }

    public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<PurchaseRequestDetail> PurchaseRequestDetails { get; } =
        new List<PurchaseRequestDetail>();
}