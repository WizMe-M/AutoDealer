namespace AutoDealer.API.BodyTypes;

public class NewUser
{
    public NewUser(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }
    public string Password { get; }
}