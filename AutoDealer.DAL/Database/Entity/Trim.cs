namespace AutoDealer.DAL.Database.Entity;

public partial class Trim
{
    public int Id { get; set; }

    [JsonIgnore]
    public int IdModel { get; set; }

    public string Code { get; set; } = null!;

    public virtual Model Model { get; set; } = null!;

    [JsonIgnore]
    public virtual IEnumerable<Auto> Autos { get; } = new List<Auto>();

    [JsonIgnore]
    public virtual IEnumerable<Margin> Margins { get; } = new List<Margin>();

    public virtual IEnumerable<TrimDetail> TrimDetails { get; } = new List<TrimDetail>();
}