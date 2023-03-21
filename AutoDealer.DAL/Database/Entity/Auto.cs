using System.ComponentModel.DataAnnotations;

namespace AutoDealer.DAL.Database.Entity;

public enum AutoStatus
{
    [Display(Name = "Was assembled")]
    Assembled,
    [Display(Name = "In testing")]
    Testing,
    [Display(Name = "On sale")]
    Selling,
    [Display(Name = "Sold out")]
    Sold
}

public partial class Auto
{
    public int Id { get; set; }

    [JsonIgnore] public int IdCarModel { get; set; }

    public DateTime AssemblyDate { get; set; }

    public decimal Cost { get; set; }

    public AutoStatus Status { get; set; }

    public virtual ICollection<Detail> Details { get; set; } = new List<Detail>();

    public virtual CarModel CarModel { get; set; } = null!;

    [JsonIgnore] public virtual IEnumerable<Sale> Sales { get; } = new List<Sale>();

    [JsonIgnore] public virtual IEnumerable<TestAuto> TestAutos { get; } = new List<TestAuto>();
}