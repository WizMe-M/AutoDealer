namespace AutoDealer.API.BodyTypes;

public record ContractData(int SupplierId, int StorekeeperId, DateOnly SupplyDate,
    IEnumerable<DetailCountCost> Details);