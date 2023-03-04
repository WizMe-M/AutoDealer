namespace AutoDealer.API.BodyTypes;

public record MarginData(int CarModelId, DateOnly StartsFrom, double MarginValue);

public record MarginIdentifier(int CarModelId, DateOnly StartsFrom);