namespace AutoDealer.DAL.Database.Entity;

public partial class Sale
{
    public int IdAuto { get; set; }

    public DateTime ExecutionDate { get; set; }

    public int IdClient { get; set; }

    public int IdEmployee { get; set; }

    public decimal TotalSum { get; set; }

    public virtual Auto IdAutoNavigation { get; set; } = null!;

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;
}