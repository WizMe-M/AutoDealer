namespace AutoDealer.DAL.Database.Entity;

public partial class ContractDetail
{
    [JsonIgnore] public int IdContract { get; set; }

    [JsonIgnore] public int IdDetailSeries { get; set; }

    public int Count { get; set; }

    public decimal CostPerOne { get; set; }

    [JsonIgnore] public virtual Contract Contract { get; set; } = null!;

    public virtual DetailSeries DetailSeries { get; set; } = null!;
}