namespace AutoDealer.API.BodyTypes;

public record NewPurchaseRequest(DateOnly ExpectedSupplyDate, int? IdUser,
    IEnumerable<DetailCountPair> DetailCountPairs) : ConstructableEntity<PurchaseRequest>
{
    public override PurchaseRequest Construct()
    {
        var request = new PurchaseRequest
        {
            ExpectedSupplyDate = ExpectedSupplyDate,
            IdUser = IdUser,
            // SentDate = DateTime.Now
        };
        
        foreach (var (seriesId, count) in DetailCountPairs)
        {
            request.PurchaseRequestDetails.Add(new PurchaseRequestDetail
            {
                IdDetailSeries = seriesId,
                Count = count
            });
        }

        return request;
    }
}