namespace AutoDealer.DAL.Database.Entity;

public partial class Work
{
    public int IdWorkPlan { get; set; }

    public int IdAuto { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Auto IdAutoNavigation { get; set; } = null!;

    public virtual WorkPlan IdWorkPlanNavigation { get; set; } = null!;
}