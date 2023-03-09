namespace AutoDealer.API.BodyTypes;

public record DetailCountCost(int IdDetailSeries, int Count, decimal CostPerOne);

public class DetailCountCostValidator : AbstractValidator<DetailCountCost>
{
    public DetailCountCostValidator()
    {
        RuleFor(dc => dc.IdDetailSeries)
            .GreaterThan(0);
        RuleFor(dc => dc.Count)
            .GreaterThan(0);
        RuleFor(dc => dc.CostPerOne)
            .GreaterThan(0);
    }
}