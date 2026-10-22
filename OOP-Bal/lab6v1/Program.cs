using System;
using System.Collections.Generic;
using System.IO;
class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public User(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public override string ToString()
    {
        return "ID: " + Id + ", Ім'я: " + Name;
    }
}
interface IRepository<T>
{
    T GetById(int id);
    void Add(T item);
    void Update(T item);
    void Delete(int id);
}
class InMemoryUserRepository : IRepository<User>
{
    private List<User> users = new List<User>();
    public User GetById(int id)
    {
        foreach (User user in users)
        {
            if (user.Id == id)
                return user;
        }
        throw new Exception("Користувача не знайдено");
    }
    public void Add(User user)
    {
        users.Add(user);
    }
    public void Update(User user)
    {
        foreach (User u in users)
        {
            if (u.Id == user.Id)
            {
                u.Name = user.Name;
            }
        }
    }
    public void Delete(int id)
    {
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].Id == id)
            {
                users.RemoveAt(i);
                break;
            }
        }
    }
}
class FileUserRepository : IRepository<User>
{
    private string filePath = "users.txt";
    public FileUserRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }
    }
    public User GetById(int id)
    {
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] data = line.Split('|');
            if (int.Parse(data[0]) == id)
            {
                return new User(
                    int.Parse(data[0]), data[1]);
            }
        }
        throw new Exception("Користувача не знайдено");
    }
    public void Add(User user)
    {
        using (StreamWriter writer =
            new StreamWriter(filePath, true))
        {
            writer.WriteLine(user.Id + "|" + user.Name);
        }
    }
    public void Update(User user)
    {
        List<User> users = new List<User>();
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] data = line.Split('|');
            User u = new User(
                int.Parse(data[0]), data[1]);
            if (u.Id == user.Id)
            {
                u.Name = user.Name;
            }
            users.Add(u);
        }
        File.WriteAllText(filePath, "");
        foreach (User u in users)
        {
            Add(u);
        }
    }
    public void Delete(int id)
    {
        List<User> users = new List<User>();
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] data = line.Split('|');
            User u = new User(
                int.Parse(data[0]), data[1]);

            if (u.Id != id)
            {
                users.Add(u);
            }
        }
        File.WriteAllText(filePath, "");
        foreach (User u in users)
        {
            Add(u);
        }
    }
}
abstract class DocumentProcessor
{
    protected string content = "";
    public abstract void LoadDocument(string path);
    public abstract void ProcessContent();
    public void SaveDocument(string path)
    {
        File.WriteAllText(path, content);
        Console.WriteLine("Документ збережено: " + path);
    }
}
class TextDocumentProcessor : DocumentProcessor
{
    public override void LoadDocument(string path)
    {
        content = File.ReadAllText(path);
        Console.WriteLine("Текстовий документ завантажено");
    }
    public override void ProcessContent()
    {
        content = content.ToUpper();
        Console.WriteLine("Текст переведено у великі літери");
    }
}
class XmlDocumentProcessor : DocumentProcessor
{
    public override void LoadDocument(string path)
    {
        content = File.ReadAllText(path);
        Console.WriteLine("XML-документ завантажено");
    }
    public override void ProcessContent()
    {
        content = content.Replace(
            "</user>", "<processed>true</processed></user>");
        Console.WriteLine("XML-документ оброблено");
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        List<IRepository<User>> repositories =
            new List<IRepository<User>>
        {
            new InMemoryUserRepository(),
            new FileUserRepository()
        };
        foreach (IRepository<User> repository in repositories)
        {
            User user = new User(1, "Марія");
            repository.Add(user);
            Console.WriteLine(repository.GetById(1));
            user.Name = "Олена";
            repository.Update(user);
            Console.WriteLine("Після оновлення:");
            Console.WriteLine(repository.GetById(1));
            repository.Delete(1);
            Console.WriteLine("Користувача видалено");
            Console.WriteLine();
        }
        File.WriteAllText("text.txt", "Hello world!");
        File.WriteAllText("data.xml",
            "<user><name>Maria</name></user>");
        List<DocumentProcessor> processors =
            new List<DocumentProcessor>
        {
            new TextDocumentProcessor(),
            new XmlDocumentProcessor()
        };
        string[] inputFiles =
        {
            "text.txt",
            "data.xml"
        };
        string[] outputFiles =
        {
            "result.txt",
            "result.xml"
        };
        for (int i = 0; i < processors.Count; i++)
        {
            processors[i].LoadDocument(inputFiles[i]);
            processors[i].ProcessContent();
            processors[i].SaveDocument(outputFiles[i]);
            Console.WriteLine();
        }
    }
}