namespace AutoDealer.DAL.Database.Entity;

public partial class Detail
{
    public int Id { get; set; }

    [JsonIgnore] public int IdDetailSeries { get; set; }

    [JsonIgnore] public int IdContract { get; set; }

    public int? IdAuto { get; set; }

    public decimal Cost { get; set; }

    public virtual DetailSeries DetailSeries { get; set; } = null!;

    [JsonIgnore] public virtual Contract Contract { get; set; } = null!;

    [JsonIgnore] public virtual Auto? Auto { get; set; }
}