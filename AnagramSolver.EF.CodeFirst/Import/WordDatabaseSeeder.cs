using AnagramSolver.EF.CodeFirst.Data;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.CodeFirst.Import;

public class WordDatabaseSeeder
{
    private readonly AnagramDbContext _context;
    public WordDatabaseSeeder(AnagramDbContext context)
    {
        _context = context;
    }
    public async Task ImportAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                $"Dictionary file was not found: {filePath}");
        }

        Dictionary<string, Category> categories = await _context.Categories.ToDictionaryAsync(category => category.Name, cancellationToken);

        Console.WriteLine("=== Categories ===");
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.Key} -> {category.Value.Id}");
        }

        string[] lines = await File.ReadAllLinesAsync(filePath, cancellationToken);

        Console.WriteLine("\n=== First 10 dictionary lines ===");
        foreach (string line in lines.Take(10))
        {
            Console.WriteLine(line);
        }

        var seenWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var wordsToInsert = new List<Word>();

        int malformed = 0;
        int unsupportedType = 0;
        int missingCategory = 0;
        int duplicates = 0;

        foreach (string line in lines)
        {
            string[] parts = line.Split((char[]?)null,StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (parts.Length < 2)
            {
                malformed++;
                continue;
            }

            string wordValue = parts[0];
            string wordType = parts[1];

            Console.WriteLine($"Word='{wordValue}', Type='{wordType}'");

            string? categoryName = wordType switch
            {
                "dkt" => "Noun",
                "vksm" => "Verb",
                "bdv" => "Adjective",
                _ => null
            };

            if (categoryName is null)
            {
                unsupportedType++;
                continue;
            }

            if (!categories.TryGetValue(categoryName, out Category? category))
            {
                missingCategory++;
                continue;
            }

            string duplicateKey = $"{wordValue}|{category.Id}";

            if (!seenWords.Add(duplicateKey))
            {
                duplicates++;
                continue;
            }

            wordsToInsert.Add(new Word
            {
                Value = wordValue,
                CategoryId = category.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        Console.WriteLine();
        Console.WriteLine($"Malformed: {malformed}");
        Console.WriteLine($"Unsupported types: {unsupportedType}");
        Console.WriteLine($"Missing categories: {missingCategory}");
        Console.WriteLine($"Duplicate lines: {duplicates}");
        Console.WriteLine($"Valid words before DB check: {wordsToInsert.Count}");

        HashSet<string> existingWords =
            await _context.Words
                .Select(word => word.Value + "|" + word.CategoryId)
                .ToHashSetAsync(cancellationToken);

        List<Word> newWords =
            wordsToInsert
                .Where(word => !existingWords.Contains(word.Value + "|" + word.CategoryId))
                .ToList();

        Console.WriteLine($"New words to insert: {newWords.Count}");

        const int batchSize = 1000;

        foreach (Word[] batch in newWords.Chunk(batchSize))
        {
            await _context.Words.AddRangeAsync(batch, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _context.ChangeTracker.Clear();
        }

        Console.WriteLine($"Imported {newWords.Count} words.");
    }
}