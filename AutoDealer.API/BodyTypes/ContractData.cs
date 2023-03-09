namespace AutoDealer.API.BodyTypes;

public record ContractData(int SupplierId, int StorekeeperId, DateOnly SupplyDate,
    IEnumerable<DetailCountCost> Details);

public class ContractDataValidator : AbstractValidator<ContractData>
{
    public ContractDataValidator()
    {
        RuleFor(data => data.SupplierId)
            .GreaterThan(0);
        RuleFor(data => data.StorekeeperId)
            .GreaterThan(0);
        RuleFor(data => data.SupplyDate)
            .NotEmpty();
        RuleFor(data => data.Details)
            .NotEmpty();
    }
}