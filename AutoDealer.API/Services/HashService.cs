namespace AutoDealer.API.Services;

public class HashService
{
    public string HashPassword(string password)
    {
        const string salt = "SomeStaticSalt";
        var passwordSalt = $"{password}{salt}";
        var bytes = Encoding.ASCII.GetBytes(passwordSalt);
        var hashData = SHA512.HashData(bytes);
        var hash = Convert.ToHexString(hashData);
        return hash;
    }
}