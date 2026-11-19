using System;

class InsufficientFundsException : Exception
{
    public decimal RequestedAmount { get; }
    public decimal CurrentBalance { get; }
    public InsufficientFundsException(decimal requestedAmount, decimal currentBalance)
        : base("На рахунку недостатньо коштів.")
    {
        RequestedAmount = requestedAmount;
        CurrentBalance = currentBalance;
    }
}
class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
    private Result(T value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }
   public static Result<T> Success(T value)
{
    return new Result<T>(value, true, "");
}

public static Result<T> Failure(string error)
{
    return new Result<T>(default(T)!, false, error);
}

public static Result<T> Failure(Exception ex)
{
    return new Result<T>(default(T)!, false, ex.Message);
}
}
class AccountService
{
    private decimal balance;
    public AccountService(decimal initialBalance)
    {
        balance = initialBalance;
    }
    public void Withdraw(decimal amount)
    {
        if (amount > balance)
        {
            throw new InsufficientFundsException(amount, balance);
        }
        balance -= amount;
        Console.WriteLine($"Знято: {amount} грн.");
        Console.WriteLine($"Залишок на рахунку: {balance} грн.");
    }
    public Result<decimal> Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            return Result<decimal>.Failure("Сума поповнення повинна бути більшою за нуль.");
        }
        balance += amount;
        return Result<decimal>.Success(balance);
    }
}
class Program
{
    static void Main(string[] args)
    {
        AccountService account = new AccountService(1000);
        Console.WriteLine("БАНКІВСЬКИЙ РАХУНОК");
        Console.WriteLine("Початковий баланс: 1000 грн.");
        Console.WriteLine("\nЗНЯТТЯ КОШТІВ");
        try
        {
            account.Withdraw(300);
            account.Withdraw(1000);
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
            Console.WriteLine($"Запитана сума: {ex.RequestedAmount} грн.");
            Console.WriteLine($"Поточний баланс: {ex.CurrentBalance} грн.");
        }
        Console.WriteLine("\nПОПОВНЕННЯ РАХУНКУ");
        Result<decimal> depositResult = account.Deposit(500);
        if (depositResult.IsSuccess)
        {
            Console.WriteLine("Рахунок успішно поповнено.");
            Console.WriteLine($"Новий баланс: {depositResult.Value} грн.");
        }
        else
        {
            Console.WriteLine($"Помилка: {depositResult.Error}");
        }
        Console.WriteLine("\nПЕРЕВІРКА НЕПРАВИЛЬНОЇ СУМИ");
        Result<decimal> errorResult = account.Deposit(-200);
        if (errorResult.IsSuccess)
        {
            Console.WriteLine($"Новий баланс: {errorResult.Value} грн.");
        }
        else
        {
            Console.WriteLine($"Помилка: {errorResult.Error}");
        }
        Console.WriteLine("\nПрограму завершено.");
    }
}