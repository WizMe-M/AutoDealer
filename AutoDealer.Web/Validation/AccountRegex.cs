namespace AutoDealer.Web.Validation;

public static partial class AccountRegex
{
    public static readonly Regex Correspondent = CreateCorrespondentRegex();

    public static readonly Regex Settlement = CreateSettlementRegex();

    public static readonly Regex Tin = CreateTinRegex();

    [GeneratedRegex(@"^\d{20}$")]
    private static partial Regex CreateCorrespondentRegex();

    [GeneratedRegex(@"^\d{20}$")]
    private static partial Regex CreateSettlementRegex();

    [GeneratedRegex(@"^\d{12}$")]
    private static partial Regex CreateTinRegex();
}