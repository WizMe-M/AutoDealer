namespace AutoDealer.DAL.Database.Entity;

public partial class Margin
{
    public int IdTrim { get; set; }

    public DateOnly StartDate { get; set; }

    public decimal Margin1 { get; set; }

    public virtual Trim IdTrimNavigation { get; set; } = null!;
}