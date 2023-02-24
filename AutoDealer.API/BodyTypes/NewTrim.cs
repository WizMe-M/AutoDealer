namespace AutoDealer.API.BodyTypes;

public record NewTrim(int IdModel, string Code) : ConstructableEntity<Trim>
{
    public override Trim Construct()
    {
        return new Trim
        {
            IdModel = IdModel,
            Code = Code
        };
    }
}