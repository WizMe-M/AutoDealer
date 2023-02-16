namespace AutoDealer.DAL.Entity;

public partial class PurchaseRequestDetail
{
    public int IdPurchaseRequest { get; set; }

    public int IdDetailSeries { get; set; }

    public int Count { get; set; }

    public virtual DetailSeries IdDetailSeriesNavigation { get; set; } = null!;

    public virtual PurchaseRequest IdPurchaseRequestNavigation { get; set; } = null!;
}