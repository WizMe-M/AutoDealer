namespace AutoDealer.API.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToDateTime(this DateOnly dateOnly) => dateOnly.ToDateTime(new TimeOnly(), DateTimeKind.Utc);
}