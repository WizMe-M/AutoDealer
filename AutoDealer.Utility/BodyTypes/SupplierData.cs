namespace AutoDealer.Utility.BodyTypes;

public record SupplierData(Addresses Addresses, Accounts Accounts, string Tin);

public record Addresses(string Legal, string Postal);

public record Accounts(string Correspondent, string Settlement);

public class SupplierDataValidator : AbstractValidator<SupplierData>
{
    public SupplierDataValidator()
    {
        RuleFor(data => data.Accounts).SetValidator(new AccountsValidator());
        RuleFor(data => data.Addresses).SetValidator(new AddressesValidator());
        RuleFor(data => data.Tin)
            .NotEmpty()
            .Matches(AccountRegex.Tin).WithMessage("{PropertyName} must be a 12-digits number")
            .WithName("TIN");
    }
}

public class AddressesValidator : AbstractValidator<Addresses>
{
    public AddressesValidator()
    {
        RuleFor(addresses => addresses.Legal)
            .NotEmpty()
            .Length(0, 250)
            .WithName("Legal address");
        RuleFor(addresses => addresses.Postal)
            .NotEmpty()
            .Length(0, 250)
            .WithName("Postal address");
    }
}

public class AccountsValidator : AbstractValidator<Accounts>
{
    public AccountsValidator()
    {
        RuleFor(accounts => accounts.Correspondent)
            .NotEmpty()
            .Matches(AccountRegex.Correspondent).WithMessage("{PropertyName} must be a 20-digits number")
            .WithName("Correspondent account");
        RuleFor(accounts => accounts.Settlement)
            .NotEmpty()
            .Matches(AccountRegex.Settlement).WithMessage("{PropertyName} must be a 20-digits number")
            .WithName("Settlement account");
    }
}