namespace AutoDealer.Utility.BodyTypes;

public record UserData(int EmployeeId, string Email, string Password);

public class UserDataValidator : AbstractValidator<UserData>
{
    public UserDataValidator()
    {
        RuleFor(data => data.EmployeeId)
            .GreaterThan(0);
        RuleFor(data => data.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(data => data.Password)
            .NotEmpty()
            .Matches(UserRegex.PasswordRegex);
    }
}