namespace AutoDealer.DAL.Database.Entity;

public partial class TrimDetail
{
    public int IdTrim { get; set; }

    public int IdDetailSeries { get; set; }

    public int Count { get; set; }

    public virtual DetailSeries IdDetailSeriesNavigation { get; set; } = null!;

    public virtual Trim IdTrimNavigation { get; set; } = null!;
}