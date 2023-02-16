namespace AutoDealer.DAL.Database.Entity;

public partial class Supplier
{
    public int IdSupplier { get; set; }

    public string LegalAddress { get; set; } = null!;

    public string PostalAddress { get; set; } = null!;

    public string CorrespondentAccount { get; set; } = null!;

    public string SettlementAccount { get; set; } = null!;

    public string Tin { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();
}