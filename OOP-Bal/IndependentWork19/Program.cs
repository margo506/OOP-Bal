using System;
using System.Net;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

class InsufficientFundsException : Exception
{
    public decimal RequestedAmount { get; }
    public decimal CurrentBalance { get; }
    public InsufficientFundsException()
    {
    }
    public InsufficientFundsException(string message) : base(message)
    {
    }
    public InsufficientFundsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    public InsufficientFundsException(
        string message,
        decimal requestedAmount,
        decimal currentBalance) : base(message)
    {
        RequestedAmount = requestedAmount;
        CurrentBalance = currentBalance;
    }
}
class Program
{
    static async Task Main()
    {
        Console.WriteLine("ДИЗАЙН ВИНЯТКІВ ТА POLLY");
        DemonstrateCustomException();
        RunRetryScenario();
        RunCircuitBreakerScenario();
        await RunTimeoutScenario();
        Console.WriteLine("\nВиконання програми завершено.");
    }
    static void DemonstrateCustomException()
    {
        Console.WriteLine("\n1. ВЛАСНИЙ ВИНЯТОК");
        decimal balance = 500;
        decimal requestedAmount = 800;
        try
        {
            if (requestedAmount > balance)
            {
                throw new InsufficientFundsException(
                    "Недостатньо коштів для оплати.",
                    requestedAmount,
                    balance);
            }
            Console.WriteLine("Оплату успішно виконано.");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
            Console.WriteLine($"Потрібна сума: {ex.RequestedAmount} грн");
            Console.WriteLine($"Баланс: {ex.CurrentBalance} грн");
        }
        finally
        {
            Console.WriteLine("Перевірку оплати завершено.");
        }
    }
    static void RunRetryScenario()
    {
        Console.WriteLine("\n2. СЦЕНАРІЙ RETRY: ЗВЕРНЕННЯ ДО API");
        int attempts = 0;
        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(300),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    Console.WriteLine(
                        $"Повторна спроба №{args.AttemptNumber + 1}. " +
                        $"Помилка: {args.Outcome.Exception?.Message}");
                    return default;
                }
            })
            .Build();
        try
        {
            pipeline.Execute(() =>
            {
                attempts++;
                Console.WriteLine($"Спроба звернення до API №{attempts}");
                if (attempts < 3)
                {
                    throw new HttpRequestException(
                        "Сервіс тимчасово недоступний.");
                }
                Console.WriteLine("Відповідь від API отримано успішно.");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Остаточна помилка: {ex.Message}");
        }
    }
    static void RunCircuitBreakerScenario()
    {
        Console.WriteLine("\n3. СЦЕНАРІЙ CIRCUIT BREAKER: БАЗА ДАНИХ");
        var pipeline = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>(),
                FailureRatio = 0.5,
                MinimumThroughput = 2,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(5)
            })
            .Build();
        for (int i = 1; i <= 4; i++)
        {
            try
            {
                Console.WriteLine($"\nЗапит до бази даних №{i}");
                pipeline.Execute(() =>
                {
                    Console.WriteLine("Спроба підключення...");
                    throw new HttpRequestException(
                        "Не вдалося підключитися до бази даних.");
                });
            }
            catch (BrokenCircuitException)
            {
                Console.WriteLine(
                    "Circuit Breaker відкритий. Запит тимчасово заблоковано.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
    static async Task RunTimeoutScenario()
    {
        Console.WriteLine("\n4. СЦЕНАРІЙ TIMEOUT: ОЧІКУВАННЯ ВІДПОВІДІ");
        var pipeline = new ResiliencePipelineBuilder()
            .AddTimeout(TimeSpan.FromMilliseconds(500))
            .Build();
        try
        {
            Console.WriteLine("Надсилання запиту до сервісу...");
            await pipeline.ExecuteAsync(async cancellationToken =>
            {
                Console.WriteLine("Очікування відповіді...");
                await Task.Delay(2000, cancellationToken);
                Console.WriteLine("Відповідь отримано.");
            });
        }
        catch (TimeoutRejectedException)
        {
            Console.WriteLine(
                "Час очікування вичерпано. Запит було перервано.");
        }
    }
}