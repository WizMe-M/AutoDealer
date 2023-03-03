namespace AutoDealer.API.BodyTypes;

public record ContractData(int SupplierId, int EmployeeId, DateOnly SupplyDate, IEnumerable<DetailCountCost> Details);