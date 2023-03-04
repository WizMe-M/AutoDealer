namespace AutoDealer.DAL.Database.Entity;

public partial class Sale
{
    public int IdAuto { get; set; }

    public DateTime ExecutionDate { get; set; }

    public int IdClient { get; set; }

    public int IdSeller { get; set; }

    public decimal TotalSum { get; set; }

    public virtual Auto Auto { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}