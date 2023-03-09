namespace AutoDealer.API.BodyTypes;

public record LoginUser(string Email, string Password, Post Post);

public class LoginUserValidator : AbstractValidator<LoginUser>
{
    public LoginUserValidator()
    {
        RuleFor(data => data.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(data => data.Password)
            .NotEmpty();
    }
}