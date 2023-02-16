namespace AutoDealer.DAL.Database.Entity;

public partial class Line
{
    public int IdLine { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Model> Models { get; } = new List<Model>();
}