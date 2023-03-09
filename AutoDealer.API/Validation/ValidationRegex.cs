using System.Text.RegularExpressions;

namespace AutoDealer.API.Validation;

public static partial class ValidationRegex
{
    public static readonly Regex PasswordRegex = CreatePasswordRegex();

    public static readonly Regex NameRegex = CreateNameRegex();

    public static readonly Regex PassportSeriesRegex = CreatePassportSeriesRegex();

    public static readonly Regex PassportNumberRegex = CreatePassportNumberRegex();

    public static readonly Regex PassportDepartmentCodeRegex = CreatePassportDepartmentCodeRegex();

    [GeneratedRegex(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$%^&*+\-=])(?!.*\s).{8,}$",
        RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePasswordRegex();

    [GeneratedRegex("^[А-ЯЁ][а-яё]+$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreateNameRegex();

    [GeneratedRegex(@"^\d{4}$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePassportSeriesRegex();

    [GeneratedRegex(@"^\d{6}$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePassportNumberRegex();

    [GeneratedRegex(@"^\d{3}-\d{3}$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreatePassportDepartmentCodeRegex();
}