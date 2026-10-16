using System;
class Employee
{
    private string name;
    private decimal salary;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public decimal Salary
    {
        get { return salary; }
        set { salary = value; }
    }
    public Employee(string name, decimal salary)
    {
        this.name = name;
        this.salary = salary;
    }
    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Працівник: {Name}, зарплата: {Salary} грн");
    }
    public string GetRole()
    {
        return "Працівник";
    }
}
class Manager : Employee
{
    public string Department { get; set; }
    public Manager(string name, decimal salary, string department)
        : base(name, salary)
    {
        Department = department;
    }
    public override void DisplayInfo()
    {
        Console.WriteLine(
            $"Менеджер: {Name}, зарплата: {Salary} грн, відділ: {Department}");
    }
    public void ManageTeam()
    {
        Console.WriteLine($"{Name} керує командою відділу {Department}.");
    }
    public new string GetRole()
    {
        return "Менеджер";
    }
}
class Director : Manager
{
    public decimal Bonus { get; set; }
    public Director(
        string name,
        decimal salary,
        string department,
        decimal bonus)
        : base(name, salary, department)
    {
        Bonus = bonus;
    }
    public override void DisplayInfo()
    {
        Console.WriteLine(
            $"Директор: {Name}, зарплата: {Salary} грн, " +
            $"відділ: {Department}, премія: {Bonus} грн");
    }
    public void LeadCompany()
    {
        Console.WriteLine($"{Name} керує компанією.");
    }
}
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Employee employee = new Employee("Іван", 18000m);
        Manager manager = new Manager("Олена", 30000m, "IT");
        Director director = new Director("Андрій", 50000m, "Управління", 15000m);
        Console.WriteLine("Інформація про працівників");
        employee.DisplayInfo();
        manager.DisplayInfo();
        director.DisplayInfo();
        Console.WriteLine("\nПоліморфізм");
        Employee emp1 = manager;
        Employee emp2 = director;
        emp1.DisplayInfo();
        emp2.DisplayInfo();
        Console.WriteLine("\nУнікальні методи");
        manager.ManageTeam();
        director.LeadCompany();
        Console.WriteLine("\nДемонстрація new");
        Employee employeeRef = manager;
        Manager managerRef = manager;
        Console.WriteLine("Через Employee: " + employeeRef.GetRole());
        Console.WriteLine("Через Manager: " + managerRef.GetRole());
    }
}