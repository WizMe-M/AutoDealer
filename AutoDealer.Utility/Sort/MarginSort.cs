namespace AutoDealer.Utility.Sort;

public enum MarginSort
{
    [Display(Name = "ID asc")]
    IdAsc,

    [Display(Name = "ID desc")]
    IdDesc,

    [Display(Name = "Starts from asc")]
    StartDateAsc,
    
    [Display(Name = "Starts from desc")]
    StartDateDesc,
    
    [Display(Name = "Value asc")]
    ValueAsc,
    
    [Display(Name = "Value desc")]
    ValueDesc,
}