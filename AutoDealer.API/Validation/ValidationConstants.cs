namespace AutoDealer.API.Validation;

public static class ValidationConstants
{
    public const string PasswordPattern = "^.*(?=.{8,})(?=.*[a-zA-Z])(?=.*\\d)(?=.*[!#$%&?\"]).*$";
}