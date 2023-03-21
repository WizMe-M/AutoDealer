namespace AutoDealer.DAL.Database.Entity;

public partial class CarModel
{
    public int Id { get; set; }

    public string LineName { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public string TrimCode { get; set; } = null!;

    [JsonIgnore] public virtual IEnumerable<Auto> Autos { get; } = new List<Auto>();

    [JsonIgnore] public virtual IEnumerable<Margin> Margins { get; } = new List<Margin>();

    public virtual ICollection<CarModelDetail> CarModelDetails { get; set; } = new List<CarModelDetail>();
}