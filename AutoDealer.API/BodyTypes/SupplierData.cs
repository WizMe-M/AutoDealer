namespace AutoDealer.API.BodyTypes;

public record SupplierData(Addresses Addresses, Accounts Accounts, string Tin);

public record Addresses(string Legal, string Postal);

public record Accounts(string Correspondent, string Settlement);