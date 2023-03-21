namespace AutoDealer.Web.ViewModels;

public class CreateCarModelViewModel
{
    [Required(ErrorMessage = "Please, insert line name")]
    [Display(Name = "Line name")]
    public string LineName { get; set; } = null!;

    [Required(ErrorMessage = "Please, insert model name")]
    [Display(Name = "Model name")]
    public string ModelName { get; set; } = null!;

    [Required(ErrorMessage = "Please, insert trim code")]
    [Display(Name = "Trim code")]
    public string TrimCode { get; set; } = null!;
}