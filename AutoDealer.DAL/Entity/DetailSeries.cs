namespace AutoDealer.DAL.Entity;

public partial class DetailSeries
{
    public int IdDetailSeries { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; } = new List<ContractDetail>();

    public virtual ICollection<Detail> Details { get; } = new List<Detail>();

    public virtual ICollection<PurchaseRequestDetail> PurchaseRequestDetails { get; } =
        new List<PurchaseRequestDetail>();

    public virtual ICollection<TrimDetail> TrimDetails { get; } = new List<TrimDetail>();
}