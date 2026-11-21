using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

public class Document
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public override string ToString()
    {
        return $"[{Id}] {Title}";
    }
}
public class InvertedIndex
{
    private Dictionary<string, HashSet<int>> _index;
    private Dictionary<int, Document> _documents;
    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }
    public void AddDocument(Document doc)
    {
        if (_documents.ContainsKey(doc.Id))
        {
            var oldWords = TextProcessor.RemoveStopWords(
                TextProcessor.Tokenize(_documents[doc.Id].Content)
            ).Distinct();
            foreach (var word in oldWords)
            {
                if (_index.TryGetValue(word, out var ids))
                {
                    ids.Remove(doc.Id);
                    if (ids.Count == 0)
                    {
                        _index.Remove(word);
                    }
                }
            }
        }
        _documents[doc.Id] = doc;
        var words = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(doc.Content)
        ).Distinct();
        foreach (var word in words)
        {
            if (!_index.ContainsKey(word))
            {
                _index[word] = new HashSet<int>();
            }
            _index[word].Add(doc.Id);
        }
    }
    public List<Document> SearchSingleWord(string word)
    {
        var normalizedWord = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(word)
        ).FirstOrDefault();
        if (string.IsNullOrEmpty(normalizedWord))
        {
            return new List<Document>();
        }
        if (_index.TryGetValue(normalizedWord, out var ids))
        {
            return GetDocuments(ids);
        }
        return new List<Document>();
    }
    public List<Document> SearchMultipleWords(params string[] words)
    {
        var normalizedWords = words
            .SelectMany(w => TextProcessor.RemoveStopWords(
                TextProcessor.Tokenize(w)))
            .Distinct()
            .ToList();
        if (normalizedWords.Count == 0)
        {
            return new List<Document>();
        }
        if (!_index.TryGetValue(normalizedWords[0], out var firstIds))
        {
            return new List<Document>();
        }
        var resultIds = new HashSet<int>(firstIds);
        foreach (var word in normalizedWords.Skip(1))
        {
            if (!_index.TryGetValue(word, out var ids))
            {
                return new List<Document>();
            }
            resultIds.IntersectWith(ids);
            if (resultIds.Count == 0)
            {
                break;
            }
        }
        return GetDocuments(resultIds);
    }
    public List<Document> SearchAnyWord(params string[] words)
    {
        var normalizedWords = words
            .SelectMany(w => TextProcessor.RemoveStopWords(
                TextProcessor.Tokenize(w)))
            .Distinct()
            .ToList();
        var resultIds = new HashSet<int>();
        foreach (var word in normalizedWords)
        {
            if (_index.TryGetValue(word, out var ids))
            {
                resultIds.UnionWith(ids);
            }
        }
        return GetDocuments(resultIds);
    }
    private List<Document> GetDocuments(IEnumerable<int> ids)
    {
        return ids
            .Where(id => _documents.ContainsKey(id))
            .OrderBy(id => id)
            .Select(id => _documents[id])
            .ToList();
    }
    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Keys.Count;
}
public static class TextProcessor
{
    public static IEnumerable<string> Tokenize(string text)
    {
        return Regex.Matches(
            text.ToLowerInvariant(),
            @"[\p{L}\p{N}+#]+"
        ).Select(match => match.Value);
    }
    public static IEnumerable<string> RemoveStopWords(
        IEnumerable<string> words)
    {
        var stopWords = new HashSet<string>
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };
        return words.Where(w => !stopWords.Contains(w));
    }
}
public class PerformanceTest
{
    public static void CompareSearchMethods(
        List<Document> documents,
        string searchWord)
    {
        var index = new InvertedIndex();
        foreach (var doc in documents)
        {
            index.AddDocument(doc);
        }
        const int iterations = 100;
        var linearResults = new List<Document>();
        var indexResults = new List<Document>();
        var stopwatch = new Stopwatch();
        LinearSearch(documents, searchWord);
        index.SearchSingleWord(searchWord);
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            linearResults = LinearSearch(documents, searchWord);
        }
        stopwatch.Stop();
        double linearTime = stopwatch.ElapsedTicks / (double)iterations;
        Console.WriteLine($"Linear search: {linearTime:F2} ticks");
        stopwatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            indexResults = index.SearchSingleWord(searchWord);
        }
        stopwatch.Stop();
        double indexTime = stopwatch.ElapsedTicks / (double)iterations;
        Console.WriteLine($"Inverted Index: {indexTime:F2} ticks");
        bool resultsMatch = linearResults
            .Select(d => d.Id)
            .SequenceEqual(indexResults.Select(d => d.Id));
        Console.WriteLine(
            $"Linear found: {linearResults.Count}, Index found: {indexResults.Count}"
        );
        Console.WriteLine($"Results match: {resultsMatch}");
    }
    private static List<Document> LinearSearch(
        List<Document> documents,
        string searchWord)
    {
        var normalizedWord = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(searchWord)
        ).FirstOrDefault();
        if (string.IsNullOrEmpty(normalizedWord))
        {
            return new List<Document>();
        }
        return documents.Where(d =>
            TextProcessor.RemoveStopWords(
                TextProcessor.Tokenize(d.Content)
            ).Contains(normalizedWord)
        ).ToList();
    }
}
class Program
{
    static void Main()
    {
        var documents = new List<Document>
        {
            new Document
            {
                Id = 1,
                Title = "Introduction to C#",
                Content = "C# is a modern object-oriented programming language developed by Microsoft."
            },
            new Document
            {
                Id = 2,
                Title = "Java Basics",
                Content = "Java is a popular programming language known for its portability and performance."
            },
            new Document
            {
                Id = 3,
                Title = "Python Overview",
                Content = "Python is a high-level programming language with simple syntax."
            },
            new Document
            {
                Id = 4,
                Title = "C# Advanced Features",
                Content = "C# supports LINQ, async/await, and many modern programming paradigms."
            },
            new Document
            {
                Id = 5,
                Title = "Comparing Languages",
                Content = "C#, Java, and Python are among the most popular programming languages today."
            }
        };
        var index = new InvertedIndex();
        foreach (var doc in documents)
        {
            index.AddDocument(doc);
        }
        Console.WriteLine("ІНВЕРТОВАНИЙ ІНДЕКС");
        Console.WriteLine($"Кількість документів: {index.DocumentCount}");
        Console.WriteLine($"Кількість унікальних слів: {index.UniqueWords}");
        Console.WriteLine("\nПОШУК ЗА ОДНИМ СЛОВОМ: programming");
        var singleResults = index.SearchSingleWord("programming");
        foreach (var doc in singleResults)
        {
            Console.WriteLine(doc);
        }
        Console.WriteLine("\nПОШУК ЗА ВСІМА СЛОВАМИ (AND): programming language");
        var andResults = index.SearchMultipleWords(
            "programming", "language"
        );
        foreach (var doc in andResults)
        {
            Console.WriteLine(doc);
        }
        Console.WriteLine($"Знайдено документів: {andResults.Count}");
        Console.WriteLine("\nПОШУК ЗА БУДЬ-ЯКИМ СЛОВОМ (OR): java python");
        var orResults = index.SearchAnyWord("java", "python");
        foreach (var doc in orResults)
        {
            Console.WriteLine(doc);
        }
        Console.WriteLine($"Знайдено документів: {orResults.Count}");
        Console.WriteLine("\nПОШУК: c# async");
        var advancedResults = index.SearchMultipleWords("c#", "async");
        foreach (var doc in advancedResults)
        {
            Console.WriteLine(doc);
        }
        Console.WriteLine($"Знайдено документів: {advancedResults.Count}");
        Console.WriteLine("\nПОРІВНЯННЯ ПРОДУКТИВНОСТІ");
        var documents10 = GenerateDocuments(documents, 10);
        Console.WriteLine("\nТестування на 10 документах:");
        PerformanceTest.CompareSearchMethods(documents10, "programming");
        var documents1000 = GenerateDocuments(documents, 1000);
        Console.WriteLine("\nТестування на 1000 документах:");
        PerformanceTest.CompareSearchMethods(documents1000, "programming");
    }
    static List<Document> GenerateDocuments(
        List<Document> source,
        int count)
    {
        var result = new List<Document>();
        for (int i = 0; i < count; i++)
        {
            var template = source[i % source.Count];
            result.Add(new Document
            {
                Id = i + 1,
                Title = template.Title + " " + (i + 1),
                Content = template.Content
            });
        }
        return result;
    }
}