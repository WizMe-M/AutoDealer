namespace AutoDealer.API.BodyTypes;

public record SupplierData(string LegalAddress, string PostalAddress, 
    string CorrespondentAccount, string SettlementAccount, string Tin) : ConstructableEntity<Supplier>
{
    public override Supplier Construct()
    {
        return new Supplier
        {
            LegalAddress = LegalAddress,
            PostalAddress = PostalAddress,
            CorrespondentAccount = CorrespondentAccount,
            SettlementAccount = SettlementAccount,
            Tin = Tin
        };
    }
}