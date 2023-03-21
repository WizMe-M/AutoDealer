namespace AutoDealer.Web.Utils.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AnonymousOnlyAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity is { IsAuthenticated: true })
            context.Result = new RedirectToActionResult("Index", "Home", null);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}