namespace AutoDealer.Web.Utils.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class ApiAuthExceptionFilterAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception.GetType() != typeof(ApiNotAuthorizedException)) return;

        var controller = context.HttpContext.GetRouteValue("controller") as string;
        var action = context.HttpContext.GetRouteValue("action") as string;
        var id = context.HttpContext.GetRouteValue("id") as string;
        context.Result = new RedirectToActionResult("Login", "Auth", 
            new { prevAction = action, prevController =  controller, prevId = id });
        context.ExceptionHandled = true;
    }
}