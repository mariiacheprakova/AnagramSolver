using AnagramSolver.EF.CodeFirst.Data;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
namespace AnagramSolver.EF.CodeFirst.Import;
public class WordDatabaseSeeder
{
    private readonly AnagramDbContext _context;
    public WordDatabaseSeeder(
        AnagramDbContext context)
    {
        _context = context;
    }
    public async Task ImportAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                $"Dictionary file was not found: {filePath}");
        }
        Dictionary<string, Category> categories =
            await _context.Categories
                .ToDictionaryAsync(
                    category => category.Name,
                    cancellationToken);
        string[] lines =
            await File.ReadAllLinesAsync(
                filePath,
                cancellationToken);
        var seenWords =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);
        var wordsToInsert =
            new List<Word>();
        foreach (string line in lines)
        {
            string[] parts =
                line.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries
                    | StringSplitOptions.TrimEntries);
            if (parts.Length < 2)
            {
                continue;
            }
            string wordValue =
                parts[0].Trim();
            string wordType =
                parts[1].Trim();
            string? categoryName =
                wordType switch
                {
                    "dkt" => "Noun",
                    "vksm" => "Verb",
                    "bdv" => "Adjective",
                    _ => null
                };
            if (categoryName is null)
            {
                continue;
            }
            if (!categories.TryGetValue(
                    categoryName,
                    out Category? category))
            {
                continue;
            }
            string duplicateKey =
                $"{wordValue}|{category.Id}";
            if (!seenWords.Add(duplicateKey))
            {
                continue;
            }
            wordsToInsert.Add(
                new Word
                {
                    Value = wordValue,
                    CategoryId = category.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
        }
        HashSet<string> existingWords =
            await _context.Words
                .Select(word =>
                    word.Value + "|" + word.CategoryId)
                .ToHashSetAsync(
                    cancellationToken);
        List<Word> newWords =
            wordsToInsert
                .Where(word =>
                    !existingWords.Contains(
                        word.Value + "|" + word.CategoryId))
                .ToList();
        const int batchSize = 1000;
        foreach (Word[] batch in newWords.Chunk(batchSize))
        {
            await _context.Words.AddRangeAsync(
                batch,
                cancellationToken);
            await _context.SaveChangesAsync(
                cancellationToken);
            _context.ChangeTracker.Clear();
        }
        Console.WriteLine(
            $"Imported {newWords.Count} words.");
    }
}