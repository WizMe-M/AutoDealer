namespace AutoDealer.Utility.BodyTypes;

public record TestedAuto(int AutoId, TestStatus Status);

public class TestedAutoValidator : AbstractValidator<TestedAuto>
{
    public TestedAutoValidator()
    {
        RuleFor(data => data.AutoId)
            .GreaterThan(0);
        RuleFor(data => data.Status)
            .NotEmpty();
    }
}