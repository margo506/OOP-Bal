using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace lab16v1
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Price { get; set; }
        public int CategoryId { get; set; }
    }
    class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
    class Program
    {
        static void Run<T>(string title, Func<IEnumerable<T>> query, Func<T, string> format)
        {
            Console.WriteLine(title);
            Stopwatch sw = Stopwatch.StartNew();
            List<T> result = query().ToList();
            sw.Stop();
            foreach (T item in result)
                Console.WriteLine("  " + format(item));
            Console.WriteLine($"  Час виконання: {sw.ElapsedTicks} ticks ({sw.Elapsed.TotalMilliseconds:F4} мс)\n");
        }
        static void RunQuiet<T>(string title, Func<IEnumerable<T>> query)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int count = query().ToList().Count;
            sw.Stop();
            Console.WriteLine($"  {title,-45} рядків: {count,8}, час: {sw.Elapsed.TotalMilliseconds,10:F3} мс");
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Category> categories = new List<Category>()
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Books" },
                new Category { Id = 4, Name = "Furniture" },
                new Category { Id = 5, Name = "Sports" },
                new Category { Id = 6, Name = "Toys" },
                new Category { Id = 7, Name = "Food" },
                new Category { Id = 8, Name = "Beauty" },
                new Category { Id = 9, Name = "Garden" },
                new Category { Id = 10, Name = "Auto" }
            };
            List<Product> products = new List<Product>()
            {
                new Product { Id = 1, Name = "Laptop", Price = 25000, CategoryId = 1 },
                new Product { Id = 2, Name = "Smartphone", Price = 18000, CategoryId = 1 },
                new Product { Id = 3, Name = "Headphones", Price = 1500, CategoryId = 1 },
                new Product { Id = 4, Name = "Monitor", Price = 7000, CategoryId = 1 },
                new Product { Id = 5, Name = "Keyboard", Price = 800, CategoryId = 1 },
                new Product { Id = 6, Name = "Mouse", Price = 600, CategoryId = 1 },
                new Product { Id = 7, Name = "T-shirt", Price = 500, CategoryId = 2 },
                new Product { Id = 8, Name = "Jeans", Price = 1200, CategoryId = 2 },
                new Product { Id = 9, Name = "Jacket", Price = 2500, CategoryId = 2 },
                new Product { Id = 10, Name = "Sneakers", Price = 3000, CategoryId = 2 },
                new Product { Id = 11, Name = "Novel", Price = 350, CategoryId = 3 },
                new Product { Id = 12, Name = "Dictionary", Price = 600, CategoryId = 3 },
                new Product { Id = 13, Name = "Programming Book", Price = 900, CategoryId = 3 },
                new Product { Id = 14, Name = "Office Chair", Price = 4500, CategoryId = 4 },
                new Product { Id = 15, Name = "Desk", Price = 6000, CategoryId = 4 },
                new Product { Id = 16, Name = "Wardrobe", Price = 8500, CategoryId = 4 },
                new Product { Id = 17, Name = "Tablet", Price = 12000, CategoryId = 1 },
                new Product { Id = 18, Name = "Lamp", Price = 700, CategoryId = 4 },
                new Product { Id = 19, Name = "Football", Price = 650, CategoryId = 5 },
                new Product { Id = 20, Name = "Lego Set", Price = 2200, CategoryId = 6 },
                new Product { Id = 21, Name = "Coffee Beans", Price = 450, CategoryId = 7 },
                new Product { Id = 22, Name = "Face Cream", Price = 900, CategoryId = 8 },
                new Product { Id = 23, Name = "Garden Hose", Price = 400, CategoryId = 9 },
                new Product { Id = 24, Name = "Car Vacuum", Price = 1300, CategoryId = 10 }
            };
            _ = products.Where(p => p.Price > 0).Select(p => p.Name).ToList();
            _ = products.Join(categories, p => p.CategoryId, c => c.Id, (p, c) => p.Name).ToList();
            _ = products.GroupBy(p => p.CategoryId).Select(g => g.Average(p => p.Price)).ToList();
            _ = products.OrderByDescending(p => p.Price).First();
            Console.WriteLine("ІНТЕРНЕТ-МАГАЗИН");
            Console.WriteLine("1. Товари категорії Electronics з ціною більше 500");
            Run("1а. Синтаксис запитів:",
                () => from product in products
                      join category in categories on product.CategoryId equals category.Id
                      where category.Name == "Electronics" && product.Price > 500
                      select product,
                p => $"{p.Name} - {p.Price} грн");
            Run("1б. Синтаксис методів:",
                () =>
                {
                    int electronicsId = categories.First(c => c.Name == "Electronics").Id;
                    return products.Where(p => p.CategoryId == electronicsId && p.Price > 500);
                },
                p => $"{p.Name} - {p.Price} грн");
            Console.WriteLine("2. Товари та їхні категорії");
            Run("2а. Синтаксис запитів:",
                () => from product in products
                      join category in categories on product.CategoryId equals category.Id
                      select new { ProductName = product.Name, CategoryName = category.Name },
                x => $"{x.ProductName} - {x.CategoryName}");
            Run("2б. Синтаксис методів:",
                () => products.Join(categories,
                    p => p.CategoryId,
                    c => c.Id,
                    (p, c) => new { ProductName = p.Name, CategoryName = c.Name }),
                x => $"{x.ProductName} - {x.CategoryName}");
            Console.WriteLine("3. Середня ціна товарів у кожній категорії");
            Run("3а. Синтаксис запитів:",
                () => from product in products
                      join category in categories on product.CategoryId equals category.Id
                      group product by category.Name into productGroup
                      select new { CategoryName = productGroup.Key, AveragePrice = productGroup.Average(p => p.Price) },
                x => $"{x.CategoryName}: {x.AveragePrice:F2} грн");
            Run("3б. Синтаксис методів:",
                () => products
                    .GroupBy(p => p.CategoryId)
                    .Join(categories,
                        g => g.Key,
                        c => c.Id,
                        (g, c) => new { CategoryName = c.Name, AveragePrice = g.Average(p => p.Price) }),
                x => $"{x.CategoryName}: {x.AveragePrice:F2} грн");
            Console.WriteLine("4. Найдорожчий товар");
            Run("4а. Синтаксис запитів (orderby + First):",
                () => new[] { (from product in products orderby product.Price descending select product).First() },
                p => $"{p.Name} - {p.Price} грн");
            Run("4б. Синтаксис методів (OrderByDescending + First):",
                () => new[] { products.OrderByDescending(p => p.Price).First() },
                p => $"{p.Name} - {p.Price} грн");
            Run("4в. Синтаксис методів (MaxBy, один прохід):",
                () => new[] { products.MaxBy(p => p.Price)! },
                p => $"{p.Name} - {p.Price} грн");
            Console.WriteLine("Відкладене виконання:");
            var deferred = products.Where(p => p.Price > 1000);
            Console.WriteLine($"  Товарів дорожче 1000 до додавання: {deferred.Count()}");
            products.Add(new Product { Id = 25, Name = "Camera", Price = 9000, CategoryId = 1 });
            Console.WriteLine($"  Після додавання нового товару:      {deferred.Count()}  (запит виконується заново при кожному переборі)");
            products.RemoveAt(products.Count - 1);
            Console.WriteLine();
            Console.WriteLine("ТЕСТ НА 1 000 000 ТОВАРІВ");
            Random rnd = new Random(42);
            List<Product> big = Enumerable.Range(1, 1_000_000)
                .Select(i => new Product
                {
                    Id = i,
                    Name = "P" + i,
                    Price = rnd.Next(1, 30000),
                    CategoryId = rnd.Next(1, 11)
                })
                .ToList();
            int elId = categories.First(c => c.Name == "Electronics").Id;
            RunQuiet("1. Where (Electronics, > 500)",
                () => big.Where(p => p.CategoryId == elId && p.Price > 500));
            RunQuiet("2. Join",
                () => big.Join(categories, p => p.CategoryId, c => c.Id,
                    (p, c) => new { ProductName = p.Name, CategoryName = c.Name }));
            RunQuiet("3. GroupBy + Average",
                () => big.GroupBy(p => p.CategoryId)
                         .Select(g => new { g.Key, Avg = g.Average(p => p.Price) }));
            RunQuiet("4а. OrderByDescending + First",
                () => new[] { big.OrderByDescending(p => p.Price).First() });
            RunQuiet("4в. MaxBy",
                () => new[] { big.MaxBy(p => p.Price)! });
        }
    }
}