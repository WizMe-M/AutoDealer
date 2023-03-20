namespace AutoDealer.Utility.Validation;

public static partial class UserRegex
{
    public static readonly Regex PasswordRegex = CreatePasswordRegex();

    [GeneratedRegex(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$%^&*+\-=])(?!.*\s).{8,}$",
        RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePasswordRegex();
}