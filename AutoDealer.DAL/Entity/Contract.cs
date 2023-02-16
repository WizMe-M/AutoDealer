namespace AutoDealer.DAL.Entity;

public partial class Contract
{
    public int IdContract { get; set; }

    public int? IdEmployee { get; set; }

    public int? IdSupplier { get; set; }

    public int? IdPurchaseRequest { get; set; }

    public DateOnly ConclusionDate { get; set; }

    public DateOnly SupplyDate { get; set; }

    public decimal TotalSum { get; set; }

    public DateOnly? LadingBillIssueDate { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; } = new List<ContractDetail>();

    public virtual ICollection<Detail> Details { get; } = new List<Detail>();

    public virtual Employee? IdEmployeeNavigation { get; set; }

    public virtual PurchaseRequest? IdPurchaseRequestNavigation { get; set; }

    public virtual Supplier? IdSupplierNavigation { get; set; }
}