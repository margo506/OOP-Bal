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
        Console.WriteLine($"Файл \"{_filePath}\" відкрито.");
    }
    public void Log(string message)
    {
        if (_isFileOpen && !_disposed)
        {
            Console.WriteLine($"Лог: {message}");
        }
        else
        {
            Console.WriteLine("Неможливо записати лог: файл закрито.");
        }
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                
            }
            if (_isFileOpen)
            {
                Console.WriteLine($"Файл \"{_filePath}\" закрито.");
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