using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoDealer.Web.Utils;

public class ApiAuthExceptionFilterAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception.GetType() != typeof(ApiNotAuthorizedException)) return;

        var action = context.HttpContext.GetRouteValue("action") as string;
        var controller = context.HttpContext.GetRouteValue("controller") as string;
        context.Result = new RedirectToActionResult("Login", "Auth", 
            new { prevAction = action, prevController =  controller });
        context.ExceptionHandled = true;
    }
}