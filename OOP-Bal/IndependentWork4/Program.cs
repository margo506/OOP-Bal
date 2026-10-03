using System;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Створення товарів");
        Console.WriteLine();
        Product product1 = new Product(101, "Laptop", 35000, "Electronics", 15);
        Console.WriteLine("Товар 1 (основний конструктор):");
        Console.WriteLine(product1);
        Console.WriteLine();
        Product product2 = new Product(102, "Mouse", 800);
        Console.WriteLine("Товар 2 (скорочений конструктор):");
        Console.WriteLine(product2);
        Console.WriteLine();
        Product product3 = new Product(product1);
        Console.WriteLine("Товар 3 (конструктор копіювання):");
        Console.WriteLine(product3);
    }
}