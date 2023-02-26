namespace AutoDealer.DAL.Database.Entity;

public partial class DetailSeries
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    [JsonIgnore] public virtual IEnumerable<CarModelDetail> TrimDetails { get; } = new List<CarModelDetail>();

    [JsonIgnore] public virtual IEnumerable<ContractDetail> ContractDetails { get; } = new List<ContractDetail>();

    [JsonIgnore]
    public virtual IEnumerable<PurchaseRequestDetail> PurchaseRequestDetails { get; } =
        new List<PurchaseRequestDetail>();

    [JsonIgnore] public virtual IEnumerable<Detail> Details { get; } = new List<Detail>();
}