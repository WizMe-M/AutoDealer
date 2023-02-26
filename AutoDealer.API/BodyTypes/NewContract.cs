namespace AutoDealer.API.BodyTypes;

public record NewContract(int SupplierId, int EmployeeId, int? PurchaseRequestId, DateOnly SupplyDate,
    IEnumerable<DetailCountCost> Details) : ConstructableEntity<Contract>
{
    public override Contract Construct()
    {
        var contract = new Contract
        {
            IdEmployee = EmployeeId,
            IdSupplier = SupplierId,
            IdPurchaseRequest = PurchaseRequestId,
            SupplyDate = SupplyDate
        };

        foreach (var (series, count, cost) in Details)
        {
            contract.ContractDetails.Add(new ContractDetail
            {
                IdDetailSeries = series,
                Count = count,
                CostPerOne = cost
            });
        }

        var totalSum = Details.Select(detailCountCost =>
            detailCountCost.Count * detailCountCost.CostPerOne).Sum();
        contract.TotalSum = totalSum;

        return contract;
    }
}