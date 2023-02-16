namespace AutoDealer.DAL.Database.Entity;

public partial class Model
{
    public int IdModel { get; set; }

    public int IdLine { get; set; }

    public string Name { get; set; } = null!;

    public virtual Line IdLineNavigation { get; set; } = null!;

    public virtual ICollection<Trim> Trims { get; } = new List<Trim>();
}