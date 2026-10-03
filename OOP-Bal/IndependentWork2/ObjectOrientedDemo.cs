using System;
public static class ObjectOrientedDemo
{
    public static void Run()
    {
        Console.WriteLine("Об'єктно-орієнтована версія");
        Cart cart = new Cart();
        Product laptop = new Product("Ноутбук", 25000);
        Product mouse = new Product("Мишка", 700);
        Product headphones = new Product("Навушники", 400);
        cart.AddItem(laptop, 1);
        cart.AddItem(mouse, 2);
        cart.AddItem(headphones, 3);
        cart.PrintCart();
        double total = cart.GetTotal();
        Console.WriteLine($"Загальна сума кошика: {total:F2} грн");
    }
}