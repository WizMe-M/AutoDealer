namespace AutoDealer.DAL.Entity;

public partial class ContractDetail
{
    public int IdContract { get; set; }

    public int IdDetailSeries { get; set; }

    public int Count { get; set; }

    public decimal CostPerOne { get; set; }

    public virtual Contract IdContractNavigation { get; set; } = null!;

    public virtual DetailSeries IdDetailSeriesNavigation { get; set; } = null!;
}