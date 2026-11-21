using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Sale
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
}
class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public decimal UnitPrice { get; set; }
}
class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? City { get; set; }
}
class Program
{
    static void Main()
    {
        Random random = new Random(42);
        List<Product> products = GenerateProducts(random);
        List<Customer> customers = GenerateCustomers(random);
        List<Sale> sales = GenerateSales(random, products, customers);
        Console.WriteLine("АНАЛІТИКА ПРОДАЖІВ");
        Console.WriteLine($"Кількість продуктів: {products.Count}");
        Console.WriteLine($"Кількість клієнтів: {customers.Count}");
        Console.WriteLine($"Кількість продажів: {sales.Count}");
        RunTopProductsByCategory(sales, products);
        RunRevenueByCity(sales, products, customers);
        RunSalesByMonth(sales, products);
    }
    static List<Product> GenerateProducts(Random random)
    {
        string[] categories =
        {
            "Електроніка",
            "Одяг",
            "Продукти",
            "Побутова техніка",
            "Книги"
        };
        List<Product> products = new List<Product>();
        for (int i = 1; i <= 60; i++)
        {
            products.Add(new Product
            {
                Id = i,
                Name = "Продукт " + i,
                Category = categories[random.Next(categories.Length)],
                UnitPrice = random.Next(100, 5000)
            });
        }
        return products;
    }
    static List<Customer> GenerateCustomers(Random random)
    {
        string[] cities =
        {
            "Рівне",
            "Львів",
            "Київ",
            "Одеса",
            "Харків",
            "Тернопіль",
            "Луцьк"
        };
        List<Customer> customers = new List<Customer>();
        for (int i = 1; i <= 80; i++)
        {
            customers.Add(new Customer
            {
                Id = i,
                Name = "Клієнт " + i,
                City = cities[random.Next(cities.Length)]
            });
        }
        return customers;
    }
    static List<Sale> GenerateSales(
        Random random,
        List<Product> products,
        List<Customer> customers)
    {
        List<Sale> sales = new List<Sale>();
        for (int i = 1; i <= 500; i++)
        {
            sales.Add(new Sale
            {
                Id = i,
                ProductId = products[random.Next(products.Count)].Id,
                Quantity = random.Next(1, 11),
                SaleDate = new DateTime(2024, 1, 1)
                    .AddDays(random.Next(0, 730)),
                CustomerId = customers[random.Next(customers.Count)].Id
            });
        }
        return sales;
    }
    static void RunTopProductsByCategory(
        List<Sale> sales,
        List<Product> products)
    {
        Console.WriteLine("\n\nПАЙПЛАЙН 1: НАЙПОПУЛЯРНІШІ ПРОДУКТИ ЗА КАТЕГОРІЯМИ");
        Stopwatch stopwatch = Stopwatch.StartNew();
        var result = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalQuantity = g.Sum(s => s.Quantity)
            })
            .Join(
                products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new
                {
                    product.Name,
                    product.Category,
                    sale.TotalQuantity
                })
            .GroupBy(p => p.Category)
            .SelectMany(category => category
                .OrderByDescending(p => p.TotalQuantity)
                .Take(3))
            .OrderBy(p => p.Category)
            .ThenByDescending(p => p.TotalQuantity)
            .ToList();
        stopwatch.Stop();
        foreach (var item in result)
        {
            Console.WriteLine(
                $"{item.Category} | {item.Name} | Продано: {item.TotalQuantity}");
        }
        Console.WriteLine($"Час виконання: {stopwatch.Elapsed.TotalMilliseconds:F4} мс");
    }
    static void RunRevenueByCity(
        List<Sale> sales,
        List<Product> products,
        List<Customer> customers)
    {
        Console.WriteLine("\n\nПАЙПЛАЙН 2: ЗАГАЛЬНИЙ ДОХІД ЗА МІСТАМИ");
        Stopwatch stopwatch = Stopwatch.StartNew();
        var result = sales
            .Join(
                products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new
                {
                    sale.CustomerId,
                    Revenue = sale.Quantity * product.UnitPrice
                })
            .Join(
                customers,
                sale => sale.CustomerId,
                customer => customer.Id,
                (sale, customer) => new
                {
                    customer.City,
                    sale.Revenue
                })
            .GroupBy(s => s.City)
            .Select(g => new
            {
                City = g.Key,
                TotalRevenue = g.Sum(s => s.Revenue)
            })
            .OrderByDescending(g => g.TotalRevenue)
            .ToList();
        stopwatch.Stop();
        foreach (var item in result)
        {
            Console.WriteLine(
                $"{item.City} | Дохід: {item.TotalRevenue:N2} грн");
        }
        Console.WriteLine($"Час виконання: {stopwatch.Elapsed.TotalMilliseconds:F4} мс");
    }
    static void RunSalesByMonth(
        List<Sale> sales,
        List<Product> products)
    {
        Console.WriteLine("\n\nПАЙПЛАЙН 3: ПРОДАЖІ ЗА МІСЯЦЯМИ ТА РОКАМИ");
        Stopwatch stopwatch = Stopwatch.StartNew();
        var result = sales
            .Join(
                products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new
                {
                    sale.SaleDate,
                    sale.Quantity,
                    Revenue = sale.Quantity * product.UnitPrice
                })
            .GroupBy(s => new
            {
                Year = s.SaleDate.Year,
                Month = s.SaleDate.Month
            })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalQuantity = g.Sum(s => s.Quantity),
                TotalRevenue = g.Sum(s => s.Revenue)
            })
            .OrderBy(g => g.Year)
            .ThenBy(g => g.Month)
            .ToList();
        stopwatch.Stop();
        foreach (var item in result)
        {
            Console.WriteLine(
                $"{item.Month:D2}.{item.Year} | " +
                $"Продано одиниць: {item.TotalQuantity} | " +
                $"Дохід: {item.TotalRevenue:N2} грн");
        }
        Console.WriteLine($"Час виконання: {stopwatch.Elapsed.TotalMilliseconds:F4} мс");
    }
}