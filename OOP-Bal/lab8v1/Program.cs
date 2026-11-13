using System;
using System.Collections.Generic;
class Product
{
    public string Name;
    public double Price;
    public Product(string name, double price)
    {
        Name = name;
        Price = price;
    }
}
static class CollectionHelper<T>
{
    public static T FindMaxBy<TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        using (var enumerator = collection.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException("Колекція порожня!");
            }
            T max = enumerator.Current;
            TKey maxKey = keySelector(max);
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                TKey currentKey = keySelector(current);
                if (currentKey.CompareTo(maxKey) > 0)
                {
                    max = current;
                    maxKey = currentKey;
                }
            }
            return max;
        }
    }
}
class Program
{
    static void Main()
    {
        List<int> numbers = new List<int> { 12, 45, 7, 89, 23 };
        int maxNumber = CollectionHelper<int>.FindMaxBy(
            numbers, x => x);
        Console.WriteLine("Найбільше число: " + maxNumber);
        List<string> words = new List<string>
        {
            "Кіт", "Програмування", "Сонце", "Комп'ютер"
        };
        string longestWord = CollectionHelper<string>.FindMaxBy(
            words, x => x.Length);
        Console.WriteLine("Найдовше слово: " + longestWord);
        List<Product> products = new List<Product>
        {
            new Product("Телефон", 12000),
            new Product("Ноутбук", 35000),
            new Product("Навушники", 2500),
            new Product("Планшет", 18000)
        };
        Product expensiveProduct = CollectionHelper<Product>.FindMaxBy(
            products, x => x.Price);
        Console.WriteLine("Найдорожчий товар: " +
            expensiveProduct.Name + " - " + expensiveProduct.Price + " грн");
    }
}