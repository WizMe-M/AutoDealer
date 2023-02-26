namespace AutoDealer.API.BodyTypes;

public record CarModelData(string Line, string Model, string Code) : ConstructableEntity<CarModel>
{
    public override CarModel Construct()
    {
        return new CarModel
        {
            LineName = Line,
            ModelName = Model,
            TrimCode = Code
        };
    }
}