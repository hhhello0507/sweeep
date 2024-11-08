using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Documents;

namespace sweeep.Local;

public class CategoryCache
{
    private const string FilePath = "category.txt";

    public static CategoryCache Instance = new();

    private CategoryCache()
    {
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath);
            Console.WriteLine($"CategoryCache.init - Added {FilePath}");
        }
    }

    public List<Category> GetAll()
    {
        var readText = File.ReadAllText(FilePath);
        return readText
            .Split(",")
            .Where(category => category != "")
            .Select(category => new Category(name: category))
            .ToList();
    }

    public void Insert(string name)
    {
        File.AppendAllText(FilePath, $",{name}");
    }

    public void Delete(string _category)
    {
        var readText = File.ReadAllText(FilePath);
        var saveText = String.Join(
            ",",
            readText
                .Split(",")
                .Where(category => category != "" && category != _category)
        );
        File.WriteAllText(FilePath, saveText);
    }
}