namespace AutoDealer.API.BodyTypes;

public record NewUser(int EmployeeId, string Email, string Password) : ConstructableEntity<User>
{
    public override User Construct()
    {
        return new User
        {
            IdEmployee = EmployeeId,
            Email = Email,
            PasswordHash = Password
        };
    }
}