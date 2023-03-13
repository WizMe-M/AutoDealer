namespace AutoDealer.Web.BodyTypes;

public record MarginData(int CarModelId, DateOnly StartsFrom, double MarginValue);

public class MarginDataValidator : AbstractValidator<MarginData>
{
    public MarginDataValidator()
    {
        RuleFor(data => data.CarModelId)
            .GreaterThan(0);
        RuleFor(data => data.StartsFrom)
            .NotEmpty()
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today));
        RuleFor(data => data.MarginValue)
            .NotEmpty()
            .GreaterThan(10);
    }
}