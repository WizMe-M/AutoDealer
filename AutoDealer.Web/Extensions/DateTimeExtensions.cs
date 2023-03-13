namespace AutoDealer.Web.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToDateTime(this DateOnly dateOnly) => dateOnly.ToDateTime(new TimeOnly());
}