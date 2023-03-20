namespace AutoDealer.Web.ViewModels;

public class CreateEmployeeViewModel
{
    [Required(ErrorMessage = "Please, insert first name")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Please, insert last name")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Middle name")]
    public string? MiddleName { get; set; }
    
    [Required(ErrorMessage = "Please, insert passport series")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Passport series not matching expression '1234'")]
    [Display(Name = "Passport series")]
    public string PassportSeries { get; set; } = null!;

    [Required(ErrorMessage = "Please, insert passport number")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Passport series not matching expression '123456'")]
    [Display(Name = "Passport number")]
    public string PassportNumber { get; set; } = null!;
    
    [Required(ErrorMessage = "Please, select post")]
    [Display(Name = "Post")]
    public Post Post { get; set; }
}