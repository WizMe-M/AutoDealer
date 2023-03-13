namespace AutoDealer.Web.Extensions;

public static class MvcOptionsExtensions
{
    public static void UseGeneralRoutePrefix(this MvcOptions options, IRouteTemplateProvider routeAttribute)
    {
        options.Conventions.Add(new RoutePrefixConvention(routeAttribute));
    }

    public static void UseGeneralRoutePrefix(this MvcOptions options, string prefix)
    {
        options.UseGeneralRoutePrefix(new RouteAttribute(prefix));
    }
}