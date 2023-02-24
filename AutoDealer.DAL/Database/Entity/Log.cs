﻿namespace AutoDealer.DAL.Database.Entity;

public enum LogType
{
    Error,
    Normal
}

public partial class Log
{
    public int Id { get; set; }

    public DateTime Time { get; set; }

    public string Text { get; set; } = null!;

    public LogType Type { get; set; }
}