namespace AutoDealer.API.BodyTypes;

public class NewUser
{
    public NewUser(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; }
    public string Password { get; }
}