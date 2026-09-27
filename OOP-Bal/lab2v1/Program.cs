using System;
using System.Text;

class Book
{
    private string title;
    private string author;
    private int year;
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    public string Author
    {
        get { return author; }
        set { author = value; }
    }
    public int Year
    {
        get { return year; }
        set
        {
            if (value <= DateTime.Now.Year)
                year = value;
            else
                year = DateTime.Now.Year;
        }
    }
    public Book() : this("Unknown", "Unknown", DateTime.Now.Year)
    {
        Console.WriteLine("Викликано конструктор за замовчуванням.");
    }
    public Book(string title, string author, int year)
    {
        Console.WriteLine("Викликано параметризований конструктор.");
        this.title = title;
        this.author = author;
        Year = year;
    }
    public string GetFullInfo()
    {
        return $"Назва: {Title}, Автор: {Author}, Рік: {Year}";
    }
    ~Book()
    {
        Console.WriteLine($"Книга \"{Title}\" знищена.");
    }
}
class Program
{
    static void Main(string[] args)
{
    Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
    CreateBooks();
    Console.WriteLine();
    Console.WriteLine("Кінець Main, запускаємо GC");
    GC.Collect();
    GC.WaitForPendingFinalizers();
    Console.WriteLine("Збирач сміття завершив роботу.");
}
static void CreateBooks()
{
    Console.WriteLine("Створення об'єктів");
    Book book1 = new Book();
    Book book2 = new Book("Кобзар", "Тарас Шевченко", 1840);
    Book book3 = new Book("1984", "Джордж Орвелл", 1949);
    Console.WriteLine(book1.GetFullInfo());
    Console.WriteLine(book2.GetFullInfo());
    Console.WriteLine(book3.GetFullInfo());
}
}