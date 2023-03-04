namespace AutoDealer.DAL.Database.Entity;

public partial class Margin
{
    public int Id { get; set; }
    
    public int IdCarModel { get; set; }

    public DateOnly StartDate { get; set; }

    public decimal Value { get; set; }

    public virtual CarModel CarModel { get; set; } = null!;
}