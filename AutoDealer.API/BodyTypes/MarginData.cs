namespace AutoDealer.API.BodyTypes;

public record MarginData(int CarModelId, DateOnly StartsFrom, double MarginValue);