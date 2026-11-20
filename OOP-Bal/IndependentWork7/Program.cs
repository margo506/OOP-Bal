using System;
using System.Collections.Generic;

namespace IndependentWork7
{
    public class Cache<TKey, TValue> where TKey : notnull
    {
        private readonly int _capacity;
        private readonly Dictionary<TKey, TValue> _cache;
        private readonly Queue<TKey> _keysOrder;
        public Cache(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Ємність кешу повинна бути більшою за 0.");
            }
            _capacity = capacity;
            _cache = new Dictionary<TKey, TValue>();
            _keysOrder = new Queue<TKey>();
        }
        public void Add(TKey key, TValue value)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
                return;
            }
            if (_cache.Count >= _capacity)
            {
                TKey oldestKey = _keysOrder.Dequeue();
                _cache.Remove(oldestKey);
            }
            _cache.Add(key, value);
            _keysOrder.Enqueue(key);
        }
        public TValue Get(TKey key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                return value;
            }
            throw new KeyNotFoundException("Елемент із таким ключем не знайдено.");
        }
        public bool ContainsKey(TKey key)
        {
            return _cache.ContainsKey(key);
        }
        public int Count
        {
            get { return _cache.Count; }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Cache<string, string> cache = new Cache<string, string>(3);
            cache.Add("1", "Apple");
            cache.Add("2", "Banana");
            cache.Add("3", "Orange");
            Console.WriteLine("Початковий кеш:");
            Console.WriteLine(cache.Get("1"));
            Console.WriteLine(cache.Get("2"));
            Console.WriteLine(cache.Get("3"));
            Console.WriteLine("\nДодаємо новий елемент:");
            cache.Add("4", "Grapes");
            Console.WriteLine("Кількість елементів: " + cache.Count);
            Console.WriteLine("\nПеревірка ключів:");
            Console.WriteLine("Ключ 1: " + cache.ContainsKey("1"));
            Console.WriteLine("Ключ 2: " + cache.ContainsKey("2"));
            Console.WriteLine("Ключ 4: " + cache.ContainsKey("4"));
            Console.WriteLine("\nОтримання нового елемента:");
            Console.WriteLine(cache.Get("4"));
        }
    }
}