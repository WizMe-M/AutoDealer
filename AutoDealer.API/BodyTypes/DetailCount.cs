namespace AutoDealer.API.BodyTypes;

public record DetailCount(int DetailSeriesId, int Count);

public class DetailCountValidator : AbstractValidator<DetailCount>
{
    public DetailCountValidator()
    {
        RuleFor(dc => dc.DetailSeriesId)
            .GreaterThan(0);
        RuleFor(dc => dc.Count)
            .GreaterThan(0);
    }
}