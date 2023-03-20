using System.Reflection;

namespace AutoDealer.Web.Utils.Extensions;

public static class EnumExtension
{
    public static string DisplayName(this Enum value)
    {
        var attr = value.GetAttribute<DisplayAttribute>();
        return attr?.Name ?? "Undefined";
    }

    public static TAttribute? GetAttribute<TAttribute>(this Enum value) 
        where TAttribute : Attribute
    {
        return value.GetType()
            .GetMember(value.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
    }
}