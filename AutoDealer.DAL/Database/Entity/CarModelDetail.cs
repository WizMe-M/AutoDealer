namespace AutoDealer.DAL.Database.Entity;

public partial class CarModelDetail
{
    [JsonIgnore] public int IdCarModel { get; set; }

    [JsonIgnore] public int IdDetailSeries { get; set; }

    public int Count { get; set; }

    public virtual DetailSeries DetailSeries { get; set; } = null!;

    [JsonIgnore] public virtual CarModel CarModel { get; set; } = null!;
}