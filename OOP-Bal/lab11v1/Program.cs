using System;
using System.Collections.Generic;
using System.Linq;

namespace lab11v1
{
    class Chapter
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int PageCount { get; set; }
        public Chapter(int id, string title, int pageCount)
        {
            Id = id;
            Title = title;
            PageCount = pageCount;
        }
    }
    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public List<Chapter> Chapters { get; set; }
        public Book(int id, string title, params Chapter[] chapters)
        {
            Id = id;
            Title = title;
            Chapters = chapters.ToList();
        }
    }
    class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Book> Books { get; set; }
        public Author(int id, string name, params Book[] books)
        {
            Id = id;
            Name = name;
            Books = books.ToList();
        }
    }
    static class ChapterExtensions
    {
        public static int TotalPages(this IEnumerable<Chapter> chapters)
        {
            return chapters.Sum(c => c.PageCount);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Author> authors = new List<Author>()
            {
                new Author(1, "Robert Martin",
                    new Book(1, "Clean Code",
                        new Chapter(1, "Clean Code", 20),
                        new Chapter(2, "Meaningful Names", 25),
                        new Chapter(3, "Functions", 30)),
                    new Book(2, "Clean Architecture",
                        new Chapter(4, "Design and Architecture", 18),
                        new Chapter(5, "Programming Paradigms", 22),
                        new Chapter(6, "SOLID Principles", 35)),
                    new Book(3, "The Clean Coder",
                        new Chapter(7, "Professionalism", 15),
                        new Chapter(8, "Saying No", 20),
                        new Chapter(9, "Time Management", 24))),
                new Author(2, "Jon Skeet",
                    new Book(4, "C# in Depth",
                        new Chapter(10, "The Changing Face of C#", 28),
                        new Chapter(11, "Core Foundations", 32),
                        new Chapter(12, "Delegates", 26)),
                    new Book(5, "Real-World Collections",
                        new Chapter(13, "Lists and Arrays", 21),
                        new Chapter(14, "Dictionaries", 19),
                        new Chapter(15, "Sets", 17)),
                    new Book(6, "LINQ Explained",
                        new Chapter(16, "Query Basics", 23),
                        new Chapter(17, "Joins and Grouping", 29),
                        new Chapter(18, "Deferred Execution", 27))),
                new Author(3, "Andrew Troelsen",
                    new Book(7, "Pro C#",
                        new Chapter(19, "The C# Language", 31),
                        new Chapter(20, "Object-Oriented Programming", 40),
                        new Chapter(21, "Generics", 34)),
                    new Book(8, "Database Access",
                        new Chapter(22, "ADO.NET Basics", 28),
                        new Chapter(23, "Entity Framework", 36),
                        new Chapter(24, "Transactions", 20)),
                    new Book(9, "Async Programming",
                        new Chapter(25, "Tasks", 22),
                        new Chapter(26, "Async and Await", 26),
                        new Chapter(27, "Parallel LINQ", 19))),
                new Author(4, "Martin Fowler",
                    new Book(10, "Refactoring",
                        new Chapter(28, "Basic Refactorings", 29),
                        new Chapter(29, "Code Smells", 24),
                        new Chapter(30, "Testing", 21)),
                    new Book(11, "Patterns of Enterprise Architecture",
                        new Chapter(31, "Domain Logic", 33),
                        new Chapter(32, "Data Source Patterns", 38),
                        new Chapter(33, "Web Presentation", 27)),
                    new Book(12, "Software Design",
                        new Chapter(34, "Modularity", 18),
                        new Chapter(35, "Coupling and Cohesion", 23),
                        new Chapter(36, "Evolutionary Design", 25)))
            };
            Console.WriteLine("1. Усі розділи всіх книг усіх авторів в одному списку");
            Console.WriteLine("Синтаксис методів:");
            var chapters = authors
                .SelectMany(a => a.Books)
                .SelectMany(b => b.Chapters)
                .ToList();
            foreach (var c in chapters)
                Console.WriteLine($"  {c.Id,2}. {c.Title} ({c.PageCount} стор.)");
            Console.WriteLine($"Усього розділів: {chapters.Count}\n");
            Console.WriteLine("Синтаксис запитів:");
            var chaptersQuery = from a in authors
                                from b in a.Books
                                from c in b.Chapters
                                select c;
            Console.WriteLine($"Усього розділів: {chaptersQuery.Count()}\n");
            Console.WriteLine("2. Усі книги по 3 на сторінку");
            var books = authors.SelectMany(a => a.Books).ToList();
            int pageSize = 3;
            int totalPages = (int)Math.Ceiling(books.Count / (double)pageSize);
            for (int page = 1; page <= totalPages; page++)
            {
                var pageItems = books
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                Console.WriteLine($"Сторінка {page} з {totalPages}:");
                foreach (var b in pageItems)
                    Console.WriteLine($"  {b.Id,2}. {b.Title}");
                Console.WriteLine();
            }
            Console.WriteLine("3. Метод розширення TotalPages() для IEnumerable<Chapter>");
            Console.WriteLine($"Загальна кількість сторінок у всіх розділах: {chapters.TotalPages()}");
            Book firstBook = authors[0].Books[0];
            Console.WriteLine($"Книга \"{firstBook.Title}\": {firstBook.Chapters.TotalPages()} стор.");
            Console.WriteLine("\nКількість сторінок по авторах:");
            foreach (var author in authors)
            {
                int pages = author.Books.SelectMany(b => b.Chapters).TotalPages();
                Console.WriteLine($"  {author.Name}: {pages} стор.");
            }
            Console.WriteLine("\nРозділи довші за 30 сторінок (Where + TotalPages):");
            var longChapters = chapters.Where(c => c.PageCount > 30);
            Console.WriteLine($"  Розділів: {longChapters.Count()}, разом {longChapters.TotalPages()} стор.");
        }
    }
}