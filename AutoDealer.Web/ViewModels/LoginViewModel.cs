using System.ComponentModel.DataAnnotations;

namespace AutoDealer.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please, insert your email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please, insert your password")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}