namespace AutoDealer.API.BodyTypes;

public record CarModelData(string Line, string Model, string Code);

public class CarModelValidator : AbstractValidator<CarModelData>
{
    public CarModelValidator()
    {
        RuleFor(data => data.Line)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(40);
        RuleFor(data => data.Model)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(40);
        RuleFor(data => data.Code)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(10);
    }
}