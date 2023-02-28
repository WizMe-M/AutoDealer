namespace AutoDealer.API.BodyTypes;

public record PurchaseRequestData(DateOnly ExpectedSupplyDate, int IdUser, IEnumerable<DetailCount> DetailCounts);