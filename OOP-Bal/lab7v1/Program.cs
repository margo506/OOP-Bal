using System;
using System.Collections.Generic;
public interface IEntity
{
    int Id { get; set; }
}
public interface IRepository<T> where T : IEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T item);
    void Update(T item);
    void Delete(int id);
}
public class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Product(int id, string name, decimal price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
    }
}
public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private List<T> items = new List<T>();
    public void Add(T item)
    {
        items.Add(item);
    }
    public T GetById(int id)
    {
        foreach (T item in items)
        {
            if (item.Id == id)
                return item;
        }
        throw new KeyNotFoundException("Елемент не знайдено.");
    }
    public IEnumerable<T> GetAll()
    {
        return items;
    }
    public void Update(T item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == item.Id)
            {
                items[i] = item;
                return;
            }
        }
        throw new KeyNotFoundException("Елемент не знайдено.");
    }
    public void Delete(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                items.RemoveAt(i);
                return;
            }
        }
        throw new KeyNotFoundException("Елемент не знайдено.");
    }
}
class Program
{
    static void Main(string[] args)
    {
        InMemoryRepository<Product> repository = new InMemoryRepository<Product>();
        repository.Add(new Product(1, "Ноутбук", 25000m, 5));
        repository.Add(new Product(2, "Мишка", 500m, 20));
        Console.WriteLine("Список товарів:");
        foreach (Product product in repository.GetAll())
        {
            Console.WriteLine($"{product.Id}. {product.Name} — {product.Price} грн, кількість: {product.Stock}");
        }
    }
}