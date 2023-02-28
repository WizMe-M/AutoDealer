namespace AutoDealer.API.BodyTypes;

public record ContractData(int SupplierId, int EmployeeId, int? PurchaseRequestId, DateOnly SupplyDate,
    IEnumerable<DetailCountCost> Details);