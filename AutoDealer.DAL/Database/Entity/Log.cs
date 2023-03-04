namespace AutoDealer.DAL.Database.Entity;

public partial class Log
{
    public DateTime Time { get; set; }

    public string Text { get; set; } = null!;
}