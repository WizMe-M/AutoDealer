namespace AutoDealer.API.BodyTypes;

public record NewModel(int LineId, string Name) : ConstructableEntity<Model>
{
    public override Model Construct()
    {
        return new Model
        {
            IdLine = LineId,
            Name = Name
        };
    }
}