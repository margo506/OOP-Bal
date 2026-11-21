using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("ДОСЛІДЖЕННЯ LINQ ТА PLINQ");
        Random random = new Random(42);
        Console.WriteLine("Генерація 10 мільйонів чисел...");
        List<int> numbers = new List<int>(10_000_000);
        for (int i = 0; i < 10_000_000; i++)
        {
            numbers.Add(random.Next(1_000_000, 10_000_000));
        }
        Console.WriteLine("Дані успішно створено.\n");
        numbers.Take(1000).Where(IsPrime).Select(n => n).ToList();
        numbers.Take(1000).AsParallel().Where(IsPrime).Select(n => n).ToList();
        RunPerformanceTest(numbers.Take(1_000_000).ToList(), "1 мільйон");
        RunPerformanceTest(numbers, "10 мільйонів");
        RunThreadSafetyTest();
    }
    static bool IsPrime(int number)
    {
        if (number < 2)
            return false;
        if (number == 2)
            return true;
        if (number % 2 == 0)
            return false;
        for (int i = 3; i * i <= number; i += 2)
        {
            if (number % i == 0)
                return false;
        }
        return true;
    }
    static void RunPerformanceTest(List<int> numbers, string size)
    {
        Console.WriteLine($"\nТЕСТУВАННЯ: {size.ToUpper()} ЕЛЕМЕНТІВ");
        Stopwatch linqWatch = Stopwatch.StartNew();
        List<int> linqResult = numbers
            .Where(n => IsPrime(n))
            .Select(n => n)
            .ToList();
        linqWatch.Stop();
        Stopwatch plinqWatch = Stopwatch.StartNew();
        List<int> plinqResult = numbers
            .AsParallel()
            .Where(n => IsPrime(n))
            .Select(n => n)
            .ToList();
        plinqWatch.Stop();
        Console.WriteLine($"LINQ: {linqWatch.Elapsed.TotalMilliseconds:F2} мс");
        Console.WriteLine($"PLINQ: {plinqWatch.Elapsed.TotalMilliseconds:F2} мс");
        Console.WriteLine($"Знайдено простих чисел LINQ: {linqResult.Count}");
        Console.WriteLine($"Знайдено простих чисел PLINQ: {plinqResult.Count}");
        Console.WriteLine(
            $"Результати збігаються: {linqResult.SequenceEqual(plinqResult)}");
        if (plinqWatch.Elapsed < linqWatch.Elapsed)
        {
            double difference =
                linqWatch.Elapsed.TotalMilliseconds /
                plinqWatch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"PLINQ швидший приблизно у {difference:F2} рази.");
        }
        else
        {
            double difference =
                plinqWatch.Elapsed.TotalMilliseconds /
                linqWatch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"LINQ швидший приблизно у {difference:F2} рази.");
        }
    }
    static void RunThreadSafetyTest()
    {
        Console.WriteLine("\nПЕРЕВІРКА ПОТОКОБЕЗПЕЧНОСТІ");
        int totalElements = 100_000;
        Console.WriteLine("Небезпечний варіант:");
        for (int attempt = 1; attempt <= 3; attempt++)
        {
            int counter = 0;
            Enumerable.Range(0, totalElements)
                .AsParallel()
                .ForAll(n =>
                {
                    counter++;
                });
            Console.WriteLine(
                $"Спроба {attempt}: {counter} замість {totalElements}");
        }
        Console.WriteLine("\nВиправлений варіант:");
        int safeCounter = 0;
        Enumerable.Range(0, totalElements)
            .AsParallel()
            .ForAll(n =>
            {
                Interlocked.Increment(ref safeCounter);
            });
        Console.WriteLine($"Результат після виправлення: {safeCounter}");
        Console.WriteLine($"Очікувана кількість: {totalElements}");
        Console.WriteLine($"Результат правильний: {safeCounter == totalElements}");
    }
}