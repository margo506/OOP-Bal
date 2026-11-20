using System;
using System.Collections.Generic;

namespace IndependentWork8
{
    public class FileMetadata
    {
        public string FileName { get; set; } = "";
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Hash { get; set; } = "";
    }
    public class FileMetadataCache
    {
        private Dictionary<string, FileMetadata> _cache;
        public FileMetadataCache()
        {
            _cache = new Dictionary<string, FileMetadata>(StringComparer.OrdinalIgnoreCase);
        }
        public void Add(FileMetadata metadata)
        {
            _cache[metadata.FileName] = metadata;
        }
        public FileMetadata Get(string fileName)
        {
            if (_cache.TryGetValue(fileName, out var metadata))
            {
                return metadata;
            }
            throw new KeyNotFoundException("Файл не знайдено.");
        }
        public bool Remove(string fileName)
        {
            return _cache.Remove(fileName);
        }
        public bool Contains(string fileName)
        {
            return _cache.ContainsKey(fileName);
        }
    }
    public class TagManager
    {
        private Dictionary<string, HashSet<string>> _articleTags;
        public TagManager()
        {
            _articleTags = new Dictionary<string, HashSet<string>>();
        }
        public void AddTag(string articleId, string tag)
        {
            if (!_articleTags.ContainsKey(articleId))
            {
                _articleTags[articleId] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            _articleTags[articleId].Add(tag);
        }
        public void RemoveTag(string articleId, string tag)
        {
            if (_articleTags.ContainsKey(articleId))
            {
                _articleTags[articleId].Remove(tag);
                if (_articleTags[articleId].Count == 0)
                {
                    _articleTags.Remove(articleId);
                }
            }
        }
        public HashSet<string> GetAllUniqueTags()
        {
            HashSet<string> allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var article in _articleTags)
            {
                allTags.UnionWith(article.Value);
            }
            return allTags;
        }
        public HashSet<string> FindCommonTags(
            string articleId1, string articleId2)
        {
            HashSet<string> commonTags = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);
            if (_articleTags.ContainsKey(articleId1) && _articleTags.ContainsKey(articleId2))
            {
                commonTags = new HashSet<string>(
                    _articleTags[articleId1],
                    StringComparer.OrdinalIgnoreCase);
                commonTags.IntersectWith(_articleTags[articleId2]);
            }
            return commonTags;
        }
    }
    public class SearchIndex
    {
        private Dictionary<string, HashSet<int>> _invertedIndex;
        public SearchIndex()
        {
            _invertedIndex = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);
        }
        public void AddDocument(int docId, string content)
        {
            char[] separators = { ' ', ',', '.', '!', '?', '\n', '\t' };
            string[] words = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                string normalizedWord = word.ToLowerInvariant();
                if (!_invertedIndex.ContainsKey(normalizedWord))
                {
                    _invertedIndex[normalizedWord] = new HashSet<int>();
                }
                _invertedIndex[normalizedWord].Add(docId);
            }
        }
        public HashSet<int> Search(string word)
        {
            if (_invertedIndex.TryGetValue(word, out var documents))
            {
                return new HashSet<int>(documents);
            }
            return new HashSet<int>();
        }
        public HashSet<int> SearchMultiple(params string[] words)
        {
            if (words.Length == 0)
            {
                return new HashSet<int>();
            }
            HashSet<int> result = Search(words[0]);
            for (int i = 1; i < words.Length; i++)
            {
                result.IntersectWith(Search(words[i]));
            }
            return result;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("КЕШ МЕТАДАНИХ ФАЙЛІВ");
            FileMetadataCache fileCache = new FileMetadataCache();
            fileCache.Add(new FileMetadata
            {
                FileName = "document.txt",
                Size = 1024,
                LastModified = DateTime.Now,
                Hash = "A1B2C3"
            });
            fileCache.Add(new FileMetadata
            {
                FileName = "photo.jpg",
                Size = 2048,
                LastModified = DateTime.Now,
                Hash = "D4E5F6"
            });
            FileMetadata file = fileCache.Get("document.txt");
            Console.WriteLine("Назва файлу: " + file.FileName);
            Console.WriteLine("Розмір: " + file.Size + " байт");
            Console.WriteLine("Хеш: " + file.Hash);
            Console.WriteLine("Чи є photo.jpg у кеші: " + fileCache.Contains("photo.jpg"));
            fileCache.Remove("photo.jpg");
            Console.WriteLine("Після видалення photo.jpg: " + fileCache.Contains("photo.jpg"));
            Console.WriteLine("\nУНІКАЛЬНІ ТЕГИ");
            TagManager tagManager = new TagManager();
            tagManager.AddTag("article1", "C#");
            tagManager.AddTag("article1", ".NET");
            tagManager.AddTag("article1", "Programming");
            tagManager.AddTag("article2", "C#");
            tagManager.AddTag("article2", ".NET");
            tagManager.AddTag("article2", "Web");
            Console.WriteLine("Усі унікальні теги:");
            HashSet<string> allTags = tagManager.GetAllUniqueTags();
            foreach (string tag in allTags)
            {
                Console.WriteLine(tag);
            }
            Console.WriteLine("\nСпільні теги двох статей:");
            HashSet<string> commonTags = tagManager.FindCommonTags("article1", "article2");
            foreach (string tag in commonTags)
            {
                Console.WriteLine(tag);
            }
            Console.WriteLine("\nТеги, які не є спільними:");
            allTags.ExceptWith(commonTags);
            foreach (string tag in allTags)
            {
                Console.WriteLine(tag);
            }
            tagManager.RemoveTag("article1", "Programming");
            Console.WriteLine("\nПОШУКОВИЙ ІНДЕКС");
            SearchIndex searchIndex = new SearchIndex();
            searchIndex.AddDocument(1, "C# Dictionary and collections");
            searchIndex.AddDocument(2, "C# HashSet and Dictionary");
            searchIndex.AddDocument(3, "HTML CSS and JavaScript");
            Console.WriteLine("Пошук слова C#:");
            HashSet<int> searchResult = searchIndex.Search("C#");
            foreach (int id in searchResult)
            {
                Console.WriteLine("Документ №" + id);
            }
            Console.WriteLine("\nПошук документів зі словами C# і Dictionary:");
            HashSet<int> multipleResult = searchIndex.SearchMultiple("C#", "Dictionary");
            foreach (int id in multipleResult)
            {
                Console.WriteLine("Документ №" + id);
            }
        }
    }
}