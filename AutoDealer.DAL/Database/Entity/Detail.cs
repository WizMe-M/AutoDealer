namespace AutoDealer.DAL.Database.Entity;

public partial class Detail
{
    [JsonIgnore] public int IdDetailSeries { get; set; }

    public int IdDetail { get; set; }

    [JsonIgnore] public int IdContract { get; set; }

    public int? IdAuto { get; set; }

    public decimal Cost { get; set; }

    [JsonIgnore] public virtual Auto? Auto { get; set; }

    [JsonIgnore] public virtual Contract Contract { get; set; } = null!;

    public virtual DetailSeries DetailSeries { get; set; } = null!;
}