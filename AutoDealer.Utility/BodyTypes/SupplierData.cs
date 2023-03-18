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
            .Matches(AccountRegex.Tin);
    }
}

public class AddressesValidator : AbstractValidator<Addresses>
{
    public AddressesValidator()
    {
        RuleFor(addresses => addresses.Legal)
            .NotEmpty()
            .Length(0, 250);
        RuleFor(addresses => addresses.Postal)
            .NotEmpty()
            .Length(0, 250);
    }
}

public class AccountsValidator : AbstractValidator<Accounts>
{
    public AccountsValidator()
    {
        RuleFor(accounts => accounts.Correspondent)
            .NotEmpty()
            .Matches(AccountRegex.Correspondent);
        RuleFor(accounts => accounts.Settlement)
            .NotEmpty()
            .Matches(AccountRegex.Settlement);
    }
}