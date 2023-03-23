namespace AutoDealer.Web.Services;

public class PostgresDumpService
{
    private readonly ConnectionString _connectionString;

    public PostgresDumpService(ConnectionString connectionString)
    {
        _connectionString = connectionString;
    }

    private static bool IsUnix => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    private static string ScriptExtension => IsUnix ? "sh" : "bat";

    /// <summary>
    /// Backup a PostgreSQL database using pg_dump
    /// </summary>
    /// <param name="localDatabasePath">The local file path to the folder where the backup will be saved</param>
    public void BackupDatabase(string localDatabasePath)
    {
        if (!Directory.Exists(localDatabasePath)) return;

        TryExecuteBackup(localDatabasePath);
    }

    private void TryExecuteBackup(string path)
    {
        var pathArgument = Path.Combine(path, $"{Guid.NewGuid()}.sql");

        var (host, port, database, user, password) = _connectionString;

        var batContent = $"pg_dump -h {host} -p {port} -U {user} -F c {database} -f {pathArgument}";
        var tempBatPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.{ScriptExtension}");
        File.WriteAllText(tempBatPath, batContent, Encoding.UTF8);

        var info = IsUnix
            ? new ProcessStartInfo("sh", tempBatPath)
            : new ProcessStartInfo(tempBatPath);
        
        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        info.RedirectStandardError = true;
        info.Environment.Add("PGPASSWORD", password);

        try
        {
            using var process = Process.Start(info)!;
            process.WaitForExit();
            var result = process.ExitCode;
            process.Close();
            Debug.WriteLine($"Backup result for script {batContent} is '{result}'");
        }
        finally
        {
            if (File.Exists(tempBatPath))
                File.Delete(tempBatPath);
        }
    }
}