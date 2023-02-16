namespace AutoDealer.DAL.Entity;

public partial class WorkPlan
{
    public int IdWorkPlan { get; set; }

    public DateOnly ConclusionDate { get; set; }

    public DateOnly WorkStartDate { get; set; }

    public DateOnly WorkEndDate { get; set; }

    public virtual ICollection<Work> Works { get; } = new List<Work>();
}