namespace Sat242516008.Loggers;

public class FileLogger
{
    private readonly string _filePath;

    public FileLogger(IWebHostEnvironment env)
    {
        // Log dosyasını projenin ana dizininde "system_logs.txt" olarak tutar.
        _filePath = Path.Combine(env.ContentRootPath, "system_logs.txt");
    }

    public void Log(string message)
    {
        try
        {
            var logRecord = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}";
            File.AppendAllText(_filePath, logRecord);
        }
        catch (Exception)
        {
            // Log atarken hata olursa sistemi durdurmasın.
        }
    }
}