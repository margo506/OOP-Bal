using System;
public static class ProceduralDemo
{
    public static void Run()
    {
        string[] productNames =
        {
            "Ноутбук",
            "Мишка",
            "Навушники"
        };
        double[] prices =
        {
            25000,
            700,
            400
        };
        int[] quantities =
        {
            1,
            2,
            3
        };
        Console.WriteLine("Процедурна версія");
        double total = CalculateCartTotal(productNames, prices, quantities);
        Console.WriteLine($"Загальна сума кошика: {total:F2} грн");
    }
    private static double CalculateItemTotal(double price, int quantity)
    {
        return price * quantity;
    }
    private static double ApplyDiscount(double price, int quantity)
    {
        double itemTotal = CalculateItemTotal(price, quantity);
        if (price > 500)
        {
            double discount = itemTotal * 0.10;
            Console.WriteLine($"Знижка 10%: -{discount:F2} грн");
            return itemTotal - discount;
        }
        return itemTotal;
    }
    private static double CalculateCartTotal(
        string[] productNames,
        double[] prices,
        int[] quantities)
    {
        double total = 0;

        for (int i = 0; i < productNames.Length; i++)
        {
            double itemTotal = CalculateItemTotal(prices[i], quantities[i]);
            Console.WriteLine(
                $"{productNames[i]}: {prices[i]:F2} грн x {quantities[i]} = {itemTotal:F2} грн");

            double finalItemTotal = ApplyDiscount(prices[i], quantities[i]);
            total += finalItemTotal;
        }
        return total;
    }
}