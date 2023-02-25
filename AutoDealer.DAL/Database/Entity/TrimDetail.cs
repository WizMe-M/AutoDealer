namespace AutoDealer.DAL.Database.Entity;

public partial class TrimDetail
{
    [JsonIgnore]
    public int IdTrim { get; set; }

    [JsonIgnore]
    public int IdDetailSeries { get; set; }

    public int Count { get; set; }

    public virtual DetailSeries DetailSeries { get; set; } = null!;

    [JsonIgnore]
    public virtual Trim Trim { get; set; } = null!;
}