using System;
using System.Collections.Generic;
namespace lab15v1
{
    class Program
    {
        static HashSet<string> tags = new HashSet<string>();
        static Queue<string> taskQueue = new Queue<string>();
        static Stack<string> history = new Stack<string>();
        static void AddTag(string tag)
        {
            if (tags.Add(tag))
                Console.WriteLine("Тег додано: " + tag);
            else
                Console.WriteLine("Такий тег вже існує: " + tag);
        }
        static void AddTask(string task)
        {
            taskQueue.Enqueue(task);
            Console.WriteLine("Завдання додано: " + task);
        }
        static void ExecuteTask()
        {
            if (taskQueue.Count > 0)
            {
                string task = taskQueue.Dequeue();
                history.Push(task);
                Console.WriteLine("Виконано завдання: " + task);
            }
            else
            {
                Console.WriteLine("Черга завдань порожня.");
            }
        }
        static void UndoTask()
        {
            if (history.Count > 0)
            {
                string task = history.Pop();
                taskQueue.Enqueue(task);
                Console.WriteLine("Скасовано виконання: " + task);
            }
            else
            {
                Console.WriteLine("Історія виконаних завдань порожня.");
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("УНІКАЛЬНІ ТЕГИ");
            AddTag("Навчання");
            AddTag("Робота");
            AddTag("Навчання");
            AddTag("Програмування");
            Console.WriteLine("Усі теги:");
            foreach (string tag in tags)
            {
                Console.WriteLine(tag);
            }
            Console.WriteLine("Чи є тег 'Робота': " +
                tags.Contains("Робота"));
            tags.Remove("Робота");
            Console.WriteLine("Після видалення тегу 'Робота':");
            foreach (string tag in tags)
            {
                Console.WriteLine(tag);
            }
            Console.WriteLine("\nЧЕРГА ЗАВДАНЬ");
            AddTask("Виконати лабораторну");
            AddTask("Підготувати звіт");
            AddTask("Повторити теорію");
            Console.WriteLine("Перше завдання в черзі: " +
                taskQueue.Peek());
            ExecuteTask();
            ExecuteTask();
            Console.WriteLine("Залишилося завдань: " +
                taskQueue.Count);
            Console.WriteLine("\nІСТОРІЯ ВИКОНАНИХ ЗАВДАНЬ");
            Console.WriteLine("Останнє виконане завдання: " +
                history.Peek());
            UndoTask();
            Console.WriteLine("Після скасування:");
            foreach (string task in history)
            {
                Console.WriteLine(task);
            }
            Console.WriteLine("\nЗавдання знову в черзі:");
            foreach (string task in taskQueue)
            {
                Console.WriteLine(task);
            }
        }
    }
}