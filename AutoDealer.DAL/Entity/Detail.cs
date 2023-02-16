namespace AutoDealer.DAL.Entity;

public partial class Detail
{
    public int IdDetailSeries { get; set; }

    public int IdDetail { get; set; }

    public int IdContract { get; set; }

    public int? IdAuto { get; set; }

    public decimal Cost { get; set; }

    public virtual Auto? IdAutoNavigation { get; set; }

    public virtual Contract IdContractNavigation { get; set; } = null!;

    public virtual DetailSeries IdDetailSeriesNavigation { get; set; } = null!;
}