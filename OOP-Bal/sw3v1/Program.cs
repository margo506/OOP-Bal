using System;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Сценарій 1: використання using");
        using (FileLogger logger = new FileLogger("using.log"))
        {
            logger.Log("Повідомлення через using.");
        }
        Console.WriteLine();
        Console.WriteLine("Сценарій 2: явний виклик Dispose()");
        FileLogger logger2 = new FileLogger("manual.log");
        logger2.Log("Повідомлення перед Dispose().");
        logger2.Dispose();
        logger2.Dispose();
        Console.WriteLine();
        Console.WriteLine("Сценарій 3: без Dispose(), робота деструктора");
        CreateLoggerWithoutDispose();
        Console.WriteLine("Викликаємо GC.Collect()...");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Деструктор завершив роботу.");
    }
    static void CreateLoggerWithoutDispose()
    {
        FileLogger logger3 = new FileLogger("finalizer.log");
        logger3.Log("Повідомлення без явного Dispose().");
    }
}