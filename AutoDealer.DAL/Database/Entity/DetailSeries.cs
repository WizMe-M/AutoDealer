namespace AutoDealer.DAL.Database.Entity;

public partial class DetailSeries
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public virtual IEnumerable<TrimDetail> TrimDetails { get; } = new List<TrimDetail>();

    public virtual IEnumerable<ContractDetail> ContractDetails { get; } = new List<ContractDetail>();

    public virtual IEnumerable<PurchaseRequestDetail> PurchaseRequestDetails { get; } =
        new List<PurchaseRequestDetail>();

    public virtual IEnumerable<Detail> Details { get; } = new List<Detail>();
}