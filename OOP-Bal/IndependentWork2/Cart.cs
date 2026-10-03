using System;
using System.Collections.Generic;
class Cart
{
    private List<Product> products;
    private List<int> quantities;
    public Cart()
    {
        products = new List<Product>();
        quantities = new List<int>();
    }
    public void AddItem(Product product, int quantity)
    {
        products.Add(product);
        quantities.Add(quantity);
    }
    public double ApplyDiscount(Product product, int quantity)
    {
        double total = product.Price * quantity;
        if (product.Price > 500)
        {
            return total * 0.90;
        }
        return total;
    }
    public double GetTotal()
    {
        double total = 0;
        for (int i = 0; i < products.Count; i++)
        {
            total += ApplyDiscount(products[i], quantities[i]);
        }
        return total;
    }
    public void PrintCart()
    {
        for (int i = 0; i < products.Count; i++)
        {
            double itemTotal = products[i].Price * quantities[i];
            Console.WriteLine(
                $"{products[i].Name}: {products[i].Price:F2} грн x {quantities[i]} = {itemTotal:F2} грн");
            if (products[i].Price > 500)
            {
                double discount = itemTotal * 0.10;
                Console.WriteLine($"Знижка 10%: -{discount:F2} грн");
            }
        }
    }
}