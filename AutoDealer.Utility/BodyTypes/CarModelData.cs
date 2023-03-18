namespace AutoDealer.Utility.BodyTypes;

public record CarModelData(string Line, string Model, string Code);

public class CarModelValidator : AbstractValidator<CarModelData>
{
    public CarModelValidator()
    {
        RuleFor(data => data.Line)
            .NotEmpty()
            .Length(3, 40);
        RuleFor(data => data.Model)
            .NotEmpty()
            .Length(3, 40);
        RuleFor(data => data.Code)
            .NotEmpty()
            .Length(3, 10);
    }
}