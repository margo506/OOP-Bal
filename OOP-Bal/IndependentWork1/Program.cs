using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        Rectangle rectangle = new Rectangle(5.5, 3.0);
        Console.WriteLine();
        Console.WriteLine("Прямокутник");
        Console.WriteLine($"Ширина: {rectangle.Width}");
        Console.WriteLine($"Висота: {rectangle.Height}");
        Console.WriteLine($"Площа: {rectangle.Area:F2}");
        Console.WriteLine($"Периметр: {rectangle.CalculatePerimeter():F2}");

        Recipe recipe = new Recipe("Паста Карбонара", 25);
        Console.WriteLine();
        Console.WriteLine("Рецепт");
        Console.WriteLine($"Назва: {recipe.Name}");
        Console.WriteLine($"Час приготування: {recipe.CookingTime} хв.");
        Console.WriteLine($"Чи є рецепт швидким: {recipe.IsQuickRecipe()}");

        Playlist playlist = new Playlist("Улюблені пісні", 25);
        Console.WriteLine();
        Console.WriteLine("Плейлист");
        Console.WriteLine($"Назва: {playlist.Name}");
        Console.WriteLine($"Кількість пісень: {playlist.SongsCount}");
        Console.WriteLine($"Чи є плейлист довгим: {playlist.IsLongPlaylist()}");
    }
}
