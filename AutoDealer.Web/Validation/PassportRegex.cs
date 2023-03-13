namespace AutoDealer.Web.Validation;

public static partial class PassportRegex
{
    public static readonly Regex SeriesRegex = CreateSeriesRegex();

    public static readonly Regex NumberRegex = CreateNumberRegex();

    public static readonly Regex DepartmentCodeRegex = CreateDepartmentCodeRegex();

    [GeneratedRegex(@"^\d{4}$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreateSeriesRegex();

    [GeneratedRegex(@"^\d{6}$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreateNumberRegex();

    [GeneratedRegex(@"^\d{3}-\d{3}$", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex CreateDepartmentCodeRegex();
}