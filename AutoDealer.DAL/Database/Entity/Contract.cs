namespace AutoDealer.DAL.Database.Entity;

public partial class Contract
{
    public int Id { get; set; }

    [JsonIgnore] public int? IdEmployee { get; set; }

    [JsonIgnore] public int? IdSupplier { get; set; }

    [JsonIgnore] public int? IdPurchaseRequest { get; set; }

    public DateOnly ConclusionDate { get; set; }

    public DateOnly SupplyDate { get; set; }

    public decimal TotalSum { get; set; }

    public DateOnly? LadingBillIssueDate { get; set; }
    
    public virtual IEnumerable<ContractDetail> ContractDetails { get; } = new List<ContractDetail>();

    public virtual IEnumerable<Detail> Details { get; } = new List<Detail>();

    public virtual Employee? Employee { get; set; }

    public virtual PurchaseRequest? PurchaseRequest { get; set; }

    public virtual Supplier? Supplier { get; set; }
}