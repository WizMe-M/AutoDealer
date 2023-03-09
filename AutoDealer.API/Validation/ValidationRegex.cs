using System.Text.RegularExpressions;

namespace AutoDealer.API.Validation;

public static partial class ValidationRegex
{
    public static readonly Regex PasswordRegex = CreatePasswordRegex();

    public static readonly Regex NameRegex = CreateNameRegex();

    public static readonly Regex PassportSeriesRegex = CreatePassportSeriesRegex();

    public static readonly Regex PassportNumberRegex = CreatePassportNumberRegex();

    [GeneratedRegex(@"^\d{4}$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePassportSeriesRegex();

    [GeneratedRegex(@"^\d{6}$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePassportNumberRegex();

    [GeneratedRegex(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$%^&*+\-=])(?!.*\s).{8,}$",
        RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePasswordRegex();

    [GeneratedRegex("^[А-ЯЁ]{1}[а-яё]+$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreateNameRegex();
}