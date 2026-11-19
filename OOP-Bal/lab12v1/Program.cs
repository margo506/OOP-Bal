using System;
using System.IO;
using System.Net.Http;

class FileProcessor
{
    private int attempts = 0;
    public string ReadFile(string path)
    {
        attempts++;
        Console.WriteLine($"Читання файлу. Спроба №{attempts}");
        if (attempts <= 2)
        {
            throw new FileNotFoundException("Файл тимчасово недоступний.");
        }
        return $"Файл '{path}' успішно прочитано.";
    }
}
class NetworkClient
{
    private int attempts = 0;
    public string DownloadData(string url)
    {
        attempts++;
        Console.WriteLine($"Завантаження даних. Спроба №{attempts}");
        if (attempts <= 3)
        {
            throw new HttpRequestException("Помилка мережевого з'єднання.");
        }
        return $"Дані з '{url}' успішно завантажено.";
    }
}
public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool> ?shouldRetry = null)
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                Console.WriteLine($"Виконання операції. Спроба №{attempt + 1}");
                T result = operation();
                Console.WriteLine("Операція виконана успішно!");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    Console.WriteLine("Повторення для цієї помилки заборонено.");
                    throw;
                }
                if (attempt >= retryCount)
                {
                    Console.WriteLine("Кількість повторних спроб вичерпана.");
                    throw;
                }
                double delay = initialDelay.TotalMilliseconds *
                               Math.Pow(2, attempt);
                Console.WriteLine($"Повторна спроба через {delay} мс.");
                if (delay > 0)
                {
                    System.Threading.Thread.Sleep((int)delay);
                }
                attempt++;
            }
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        FileProcessor fileProcessor = new FileProcessor();
        NetworkClient networkClient = new NetworkClient();
        Console.WriteLine("ЧИТАННЯ ФАЙЛУ");
        try
        {
            string fileResult = RetryHelper.ExecuteWithRetry(
                () => fileProcessor.ReadFile("data.txt"),
                retryCount: 3,
                initialDelay: TimeSpan.FromMilliseconds(500),
                shouldRetry: ex => ex is FileNotFoundException
            );
            Console.WriteLine(fileResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не вдалося прочитати файл: {ex.Message}");
        }

        Console.WriteLine();

        Console.WriteLine("ЗАВАНТАЖЕННЯ МЕРЕЖЕВИХ ДАНИХ");

        try
        {
            string networkResult = RetryHelper.ExecuteWithRetry(
                () => networkClient.DownloadData("https://example.com"),
                retryCount: 4,
                initialDelay: TimeSpan.FromMilliseconds(500),
                shouldRetry: ex => ex is HttpRequestException
            );

            Console.WriteLine(networkResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не вдалося завантажити дані: {ex.Message}");
        }

        Console.WriteLine();

Console.WriteLine("ПЕРЕВІРКА SHOULDRETRY");

try
{
    RetryHelper.ExecuteWithRetry<string>(
        () =>
        {
            throw new IOException("Помилка доступу до файлу.");
        },
        retryCount: 3,
        initialDelay: TimeSpan.FromMilliseconds(500),
        shouldRetry: ex => ex is FileNotFoundException ||
                           ex is HttpRequestException
    );
}
catch (Exception ex)
{
    Console.WriteLine($"Операцію зупинено: {ex.Message}");
}
        Console.WriteLine("\nПрограму завершено.");
    }
}