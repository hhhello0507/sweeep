using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;

namespace sweeep.Local;

public class RecordCache
{
    private const string FilePath = "record.txt";
    public static RecordCache Instance = new();

    private RecordCache()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
            Console.WriteLine($"RecordCache.init - Added {FilePath}");
        }
    }

    public List<Record> GetAll()
    {
        var json = File.ReadAllText(FilePath);
        var records = JsonSerializer.Deserialize<List<Record>>(json);
        return records;
    }

    public Record Insert(Record record)
    {
        var records = GetAll();
        record.Id = records.Count == 0 ? 0 : records.Select(w => w.Id).Max() + 1; // AI;
        records.Add(record);
        var json = JsonSerializer.Serialize(records);
        File.WriteAllText(FilePath, json);
        return record;
    }

    public void Edit(Record record)
    {
        var records = GetAll();
        foreach (var t in records.Where(t => record.Id == t.Id))
        {
            t.Category = record.Category;
            t.Amount = record.Amount;
            t.Date = record.Date;
            t.Memo = record.Memo;
        }

        var json = JsonSerializer.Serialize(records);
        File.WriteAllText(FilePath, json);
    }

    public void Delete(int id)
    {
        var records = GetAll();
        var filteredRecords = records.Where(w => w.Id != id);
        var json = JsonSerializer.Serialize(filteredRecords);
        File.WriteAllText(FilePath, json);
    }
}