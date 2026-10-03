class Product
{
    private int _id;
    private string _name;
    private decimal _price;
    private string _category;
    private int _stockCount;
    public int Id => _id;
    public string Name => _name;
    public decimal Price => _price;
    public string Category => _category;
    public int StockCount => _stockCount;
    public Product(int id, string name, decimal price, string category, int stockCount)
    {
        _id = id;
        _name = name;
        _price = price;
        _category = category;
        _stockCount = stockCount;
    }

    public Product(int id, string name, decimal price)
        : this(id, name, price, "Uncategorized", 0)
    {
    }
    public Product(Product other)
        : this(
            other._id,
            other._name,
            other._price,
            other._category,
            other._stockCount)
    {
    }
    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Price: {Price:F2} грн, " +
               $"Category: {Category}, Stock: {StockCount}";
    }
}