namespace AutoDealer.API.BodyTypes;

public record NewEmployee(string FirstName, string LastName, string? MiddleName,
    string PassportSeries, string PassportNumber, string Post) : ConstructableEntity<Employee>
{
    public override Employee Construct()
    {
        return new Employee
        {
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName,
            PassportNumber = PassportNumber,
            PassportSeries = PassportSeries,
            Post = Enum.Parse<Post>(Post)
        };
    }
}