namespace AutoDealer.API.Validation;

public static partial class UserRegex
{
    public static readonly Regex PasswordRegex = CreatePasswordRegex();

    public static readonly Regex NameRegex = CreateNameRegex();

    [GeneratedRegex(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$%^&*+\-=])(?!.*\s).{8,}$",
        RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePasswordRegex();

    [GeneratedRegex("^[А-ЯЁ][а-яё]+$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreateNameRegex();
}