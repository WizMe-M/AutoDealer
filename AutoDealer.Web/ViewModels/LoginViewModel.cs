namespace AutoDealer.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please, insert your email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Please, insert your password")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Please, select some role")]
    [Display(Name = "Role")]
    public Post Role { get; set; }
}