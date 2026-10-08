using System;
public class FileLogger : IDisposable
{
    private string _filePath;
    private bool _isFileOpen;
    private bool _disposed = false;
    public string FilePath
    {
        get { return _filePath; }
        set { _filePath = value; }
    }
    public bool IsFileOpen
    {
        get { return _isFileOpen; }
    }
    public FileLogger(string filePath)
    {
        _filePath = filePath;
        _isFileOpen = true;
        Console.WriteLine($"Файл відкрито: {_filePath}");
    }
    public void Log(string message)
    {
        if (_isFileOpen)
        {
            Console.WriteLine($"Запис у файл: {message}");
        }
        else
        {
            Console.WriteLine("Файл закритий.");
        }
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Console.WriteLine("Звільнення керованих ресурсів");
            }
            if (_isFileOpen)
            {
                Console.WriteLine("Файл закрито.");
                _isFileOpen = false;
            }
            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~FileLogger()
    {
        Dispose(false);
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("Використання using:");
        using (var logger = new FileLogger("log.txt"))
        {
            logger.Log("Перше повідомлення");
        }
        Console.WriteLine();
        Console.WriteLine("Явний виклик Dispose():");
        var logger2 = new FileLogger("log2.txt");
        logger2.Log("Друге повідомлення");
        logger2.Dispose();
        Console.WriteLine();
        Console.WriteLine("Робота деструктора:");
        CreateLogger();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Готово.");
    }
    static void CreateLogger()
    {
        var logger3 = new FileLogger("log3.txt");
        logger3.Log("Третє повідомлення");
    }
}