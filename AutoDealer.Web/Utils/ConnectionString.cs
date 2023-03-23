namespace AutoDealer.Web.Utils;

public partial record ConnectionString(string Host, string Port, string Database, string User, string Password)
{
    public static ConnectionString FromString(string connectionString)
    {
        const string format = "Host={0}; Port={1}; Database={2}; User ID={3}; Password={4}";
        var pattern = NumbersInCurlyBraces().Replace(format, "(.+)");
        var values = Regex.Match(connectionString, pattern)
            .Groups.Cast<Group>()
            .Skip(1)
            .Select(x => x.Value)
            .ToArray();
        var cs = new ConnectionString(values[0], values[1], values[2], values[3], values[4]);
        return cs;
    }

    [GeneratedRegex("{\\d+}")]
    private static partial Regex NumbersInCurlyBraces();
}