using System;
using System.Collections.Generic;
using Xunit;

public class UnitTest1
{
    [Fact]
    public void Add_ShouldAddProduct()
    {
        var repository = new InMemoryRepository<Product>();
        var product = new Product(1, "Ноутбук", 25000m, 5);
        repository.Add(product);
        Assert.Equal(product, repository.GetById(1));
    }
    [Fact]
    public void GetById_ShouldReturnProduct()
    {
        var repository = new InMemoryRepository<Product>();
        var product = new Product(1, "Мишка", 500m, 10);
        repository.Add(product);
        var result = repository.GetById(1);
        Assert.Equal("Мишка", result.Name);
    }
    [Fact]
    public void GetById_ShouldThrowException()
    {
        var repository = new InMemoryRepository<Product>();
        Assert.Throws<KeyNotFoundException>(() => repository.GetById(10));
    }
    [Fact]
    public void GetAll_ShouldReturnAllProducts()
    {
        var repository = new InMemoryRepository<Product>();
        repository.Add(new Product(1, "Ноутбук", 25000m, 5));
        repository.Add(new Product(2, "Мишка", 500m, 10));
        var result = repository.GetAll();
        Assert.Equal(2, result.Count());
    }
    [Fact]
    public void Update_ShouldUpdateProduct()
    {
        var repository = new InMemoryRepository<Product>();
        repository.Add(new Product(1, "Ноутбук", 25000m, 5));
        var updatedProduct = new Product(1, "Ноутбук HP", 30000m, 8);
        repository.Update(updatedProduct);
        Assert.Equal("Ноутбук HP", repository.GetById(1).Name);
        Assert.Equal(30000m, repository.GetById(1).Price);
    }
    [Fact]
    public void Update_ShouldThrowException()
    {
        var repository = new InMemoryRepository<Product>();
        var product = new Product(10, "Телефон", 15000m, 3);
        Assert.Throws<KeyNotFoundException>(() => repository.Update(product));
    }
    [Fact]
    public void Delete_ShouldRemoveProduct()
    {
        var repository = new InMemoryRepository<Product>();
        repository.Add(new Product(1, "Ноутбук", 25000m, 5));
        repository.Delete(1);
        Assert.Empty(repository.GetAll());
    }
    [Fact]
    public void Delete_ShouldThrowException()
    {
        var repository = new InMemoryRepository<Product>();
        Assert.Throws<KeyNotFoundException>(() => repository.Delete(10));
    }
}