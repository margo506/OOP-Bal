using System;
public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random = new Random();
    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
            throw new ArgumentException("Кількість повторних спроб не може бути від'ємною.");
        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentException("Затримка не може бути від'ємною.");
        if (jitterFactor < 0 || jitterFactor > 1)
            throw new ArgumentException("JitterFactor повинен бути від 0 до 1.");
        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }
    public T Execute<T>(Func<T> operation, Func<Exception, bool> shouldRetry = null!)
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                Console.WriteLine($"Спроба виконання №{attempt + 1}");
                T result = operation();
                Console.WriteLine("Операція виконана успішно.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
                if (attempt >= _maxRetries)
                {
                    Console.WriteLine("Усі повторні спроби вичерпано.");
                    throw;
                }
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    Console.WriteLine("Цю помилку повторювати не потрібно.");
                    throw;
                }
                double delayMilliseconds =
                    _initialDelay.TotalMilliseconds *
                    Math.Pow(2, attempt) *
                    (1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor);
                TimeSpan delay = TimeSpan.FromMilliseconds(delayMilliseconds);
                Console.WriteLine($"Повторна спроба через {delay.TotalMilliseconds:F0} мс.");
                Console.WriteLine();
                System.Threading.Thread.Sleep(delay);
                attempt++;
            }
        }
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("РЕАЛІЗАЦІЯ EXPONENTIAL BACKOFF З JITTER");
        Console.WriteLine();
        RetryPolicy retryPolicy = new RetryPolicy(
            maxRetries: 3,
            initialDelay: TimeSpan.FromMilliseconds(500),
            jitterFactor: 0.2
        );
        Console.WriteLine("1. СЦЕНАРІЙ: УСПІШНЕ ВИКОНАННЯ");
        int counter = 0;
        try
        {
            string result = retryPolicy.Execute(() =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Сервіс тимчасово недоступний.");
                return "Дані успішно отримано!";
            });
            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Остаточна помилка: {ex.Message}");
        }
        Console.WriteLine();
        Console.WriteLine("2. СЦЕНАРІЙ: УСІ СПРОБИ НЕВДАЛІ");
        try
        {
            string result = retryPolicy.Execute<string>(() =>
            {
                throw new Exception("Не вдалося підключитися до сервера.");
            });
            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Операцію завершено з помилкою: {ex.Message}");
        }
        Console.WriteLine();
        Console.WriteLine("Виконання програми завершено.");
    }
}