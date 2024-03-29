﻿namespace AutoDealer.DAL.Database.Entity;

public partial class Contract
{
    public int Id { get; set; }

    [JsonIgnore] public int? IdStorekeeper { get; set; }

    [JsonIgnore] public int? IdSupplier { get; set; }

    public DateTime ConclusionDate { get; set; }

    public DateOnly SupplyDate { get; set; }

    public decimal TotalSum { get; set; }

    public DateOnly? LadingBillIssueDate { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();

    public virtual IEnumerable<Detail> Details { get; set; } = new List<Detail>();

    public virtual Employee? Storekeeper { get; set; }

    public virtual Supplier? Supplier { get; set; }
}