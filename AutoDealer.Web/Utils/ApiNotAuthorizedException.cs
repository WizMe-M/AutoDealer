namespace AutoDealer.Web.Utils;

public class ApiNotAuthorizedException : Exception
{
    public ApiNotAuthorizedException() : base("API Token was not authorized. Please Sign In")
    {
    }
}