namespace AutoDealer.DAL.Database.Entity;

public partial class PurchaseRequestDetail
{
    [JsonIgnore]
    public int IdPurchaseRequest { get; set; }

    [JsonIgnore]
    public int IdDetailSeries { get; set; }

    public int Count { get; set; }

    public virtual DetailSeries DetailSeries { get; set; } = null!;

    public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;
}