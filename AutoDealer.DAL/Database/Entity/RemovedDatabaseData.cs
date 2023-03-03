namespace AutoDealer.DAL.Database.Entity;

public enum RequestStatus
{
    Sent,
    InHandling,
    Closed
}

public partial class PurchaseRequest
{
    public int Id { get; set; }

    [JsonIgnore] public int? IdUser { get; set; }

    public DateTime SentDate { get; set; }

    public DateOnly ExpectedSupplyDate { get; set; }

    public RequestStatus Status { get; set; }

    [JsonIgnore] public virtual IEnumerable<Contract> Contracts { get; } = new List<Contract>();

    public virtual User? User { get; set; }

    public virtual ICollection<PurchaseRequestDetail> PurchaseRequestDetails { get; } =
        new List<PurchaseRequestDetail>();
}

public partial class PurchaseRequestDetail
{
    [JsonIgnore] public int IdPurchaseRequest { get; set; }

    [JsonIgnore] public int IdDetailSeries { get; set; }

    public int Count { get; set; }

    public virtual DetailSeries DetailSeries { get; set; } = null!;

    [JsonIgnore] public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;
}