using System;
using System.Collections.Generic;

public class Product
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
}
public static class CollectionUtils
{
        public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        var result = new List<TResult>();
        foreach (var item in collection)
        {
            result.Add(mapper(item));
        }
        return result;
    }
    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        T maxItem = default(T)!;
        TKey maxKey = default(TKey)!;
        bool first = true;
        foreach (var item in collection)
        {
            TKey key = keySelector(item);
            if (first || key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
                first = false;
            }
        }
        return maxItem;
    }
    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }
        return result;
    }
    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}
class Program
{
    static void Main()
    {
        var numbers = new List<int> { 5, 12, 3, 20, 8 };
        Console.WriteLine("РОБОТА З ЧИСЛАМИ");
        var doubled = CollectionUtils.Map(numbers, n => n * 2);
        Console.WriteLine("Подвоєні числа: " + string.Join(", ", doubled));
        var maxNumber = CollectionUtils.MaxBy(numbers, n => n);
        Console.WriteLine("Найбільше число: " + maxNumber);
        var evenNumbers = CollectionUtils.Filter(numbers, n => n % 2 == 0);
        Console.WriteLine("Парні числа: " + string.Join(", ", evenNumbers));
        Console.WriteLine("Усі числа:");
        CollectionUtils.ForEach(numbers, n => Console.WriteLine(n));
        var names = new List<string>
        {
            "Anna", "Maksym", "Olena", "Ivan"
        };
        Console.WriteLine("\nРОБОТА З РЯДКАМИ");
        var upperNames = CollectionUtils.Map(names, n => n.ToUpper());
        Console.WriteLine("Імена великими літерами: " +
            string.Join(", ", upperNames));
        var longestName = CollectionUtils.MaxBy(names, n => n.Length);
        Console.WriteLine("Найдовше ім'я: " + longestName);
        var shortNames = CollectionUtils.Filter(names, n => n.Length <= 4);
        Console.WriteLine("Короткі імена: " + string.Join(", ", shortNames));
        Console.WriteLine("Усі імена:");
        CollectionUtils.ForEach(names, n => Console.WriteLine(n));
        var products = new List<Product>
        {
            new Product { Name = "Laptop", Price = 25000 },
            new Product { Name = "Mouse", Price = 500 },
            new Product { Name = "Keyboard", Price = 1500 }
        };
        Console.WriteLine("\nРОБОТА З ТОВАРАМИ");
        var productNames = CollectionUtils.Map(products, p => p.Name);
        Console.WriteLine("Назви товарів: " +
            string.Join(", ", productNames));
        var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"Найдорожчий товар: {mostExpensive.Name} - {mostExpensive.Price} грн");
        var expensiveProducts = CollectionUtils.Filter(products, p => p.Price > 1000);
        Console.WriteLine("Товари дорожчі за 1000 грн:");
        CollectionUtils.ForEach(expensiveProducts,
            p => Console.WriteLine($"{p.Name} - {p.Price} грн"));
        Console.WriteLine("Усі товари:");
        CollectionUtils.ForEach(products,
            p => Console.WriteLine($"{p.Name}: {p.Price} грн"));
    }
}