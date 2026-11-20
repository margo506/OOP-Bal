using System;
using System.Collections.Generic;
using Xunit;
using IndependentWork7;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_ShouldAddElement()
        {
            Cache<string, string> cache = new Cache<string, string>(3);
            cache.Add("name", "Margarita");
            Assert.True(cache.ContainsKey("name"));
            Assert.Equal("Margarita", cache.Get("name"));
        }
        [Fact]
        public void Get_ShouldReturnValue()
        {
            Cache<int, string> cache = new Cache<int, string>(3);
            cache.Add(1, "Apple");
            string result = cache.Get(1);
            Assert.Equal("Apple", result);
        }
        [Fact]
        public void Add_WhenCapacityExceeded_ShouldRemoveOldestElement()
        {
            Cache<int, string> cache = new Cache<int, string>(2);
            cache.Add(1, "First");
            cache.Add(2, "Second");
            cache.Add(3, "Third");
            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
            Assert.Equal(2, cache.Count);
        }
        [Fact]
        public void Get_WhenKeyDoesNotExist_ShouldThrowException()
        {
            Cache<int, string> cache = new Cache<int, string>(2);
            Assert.Throws<KeyNotFoundException>(() => cache.Get(5));
        }
        [Fact]
        public void ContainsKey_ShouldCheckKey()
        {
            Cache<int, string> cache = new Cache<int, string>(2);
            cache.Add(1, "Apple");
            Assert.True(cache.ContainsKey(1));
            Assert.False(cache.ContainsKey(2));
        }
        [Fact]
        public void Count_ShouldReturnNumberOfElements()
        {
            Cache<int, string> cache = new Cache<int, string>(3);
            cache.Add(1, "One");
            cache.Add(2, "Two");
            Assert.Equal(2, cache.Count);
        }
        [Fact]
        public void Add_WhenKeyAlreadyExists_ShouldUpdateValue()
        {
            Cache<int, string> cache = new Cache<int, string>(2);
            cache.Add(1, "Old");
            cache.Add(1, "New");
            Assert.Equal("New", cache.Get(1));
            Assert.Equal(1, cache.Count);
        }
        [Fact]
        public void Constructor_WhenCapacityIsZero_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Cache<int, string>(0));
        }
    }
}