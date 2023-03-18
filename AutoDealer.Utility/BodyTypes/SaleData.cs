namespace AutoDealer.Utility.BodyTypes;

public record SaleData(int ClientId, int AutoId, int SellerId);

public class SaleDataValidator : AbstractValidator<SaleData>
{
    public SaleDataValidator()
    {
        RuleFor(data => data.AutoId)
            .GreaterThan(0);
        RuleFor(data => data.SellerId)
            .GreaterThan(0);
        RuleFor(data => data.ClientId)
            .GreaterThan(0);
    }
}