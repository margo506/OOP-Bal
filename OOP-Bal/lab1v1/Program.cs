
using System;

class Book
{
    // Приватні поля
    private string title;
    private string author;

    // Публічні властивості
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

    public int Year { get; set; }

    // Конструктор
    public Book(string title, string author, int year)
    {
        this.title = title;
        this.author = author;
        Year = year;
    }

    // Метод
    public string GetInfo()
    {
        return $"Назва: {title}, Автор: {author}, Рік: {Year}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Book book1 = new Book("Кобзар", "Тарас Шевченко", 1840);
        Book book2 = new Book("Лісова пісня", "Леся Українка", 1911);
        Book book3 = new Book("1984", "Джордж Орвелл", 1949);

        Console.WriteLine(book1.GetInfo());
        Console.WriteLine(book2.GetInfo());
        Console.WriteLine(book3.GetInfo());
    }
}
