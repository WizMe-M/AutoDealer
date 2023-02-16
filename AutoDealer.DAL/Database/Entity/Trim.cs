namespace AutoDealer.DAL.Database.Entity;

public partial class Trim
{
    public int IdTrim { get; set; }

    public int IdModel { get; set; }

    public string Code { get; set; } = null!;

    public virtual ICollection<Auto> Autos { get; } = new List<Auto>();

    public virtual Model IdModelNavigation { get; set; } = null!;

    public virtual ICollection<Margin> Margins { get; } = new List<Margin>();

    public virtual ICollection<TrimDetail> TrimDetails { get; } = new List<TrimDetail>();
}