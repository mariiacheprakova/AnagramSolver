using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using DomainWord = AnagramSolver.Contracts.Models.Word;
using EfWord = AnagramSolver.EF.CodeFirst.Models.Word;

namespace AnagramSolver.EF.CodeFirst.Repositories;
public class EfWordRepository : IWordRepository
{
    private readonly AnagramDbContext _context;
    public EfWordRepository(AnagramDbContext context)
    {
        _context = context;
    }
    public async Task<DomainWord[]> GetAllWordsAsync(CancellationToken cancellationToken = default)
    {
        EfWord[] databaseWords =
            await _context.Words
                .AsNoTracking()
                .Include(word => word.Category)
                .Where(word => word.IsActive)
                .ToArrayAsync(cancellationToken);

        return databaseWords.Select(MapToDomainWord).ToArray();
    }

    public async Task<DomainWord?> GetWordByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        EfWord? databaseWord =
            await _context.Words
                .AsNoTracking()
                .Include(word => word.Category)
                .FirstOrDefaultAsync(
                    word => word.Id == id,
                    cancellationToken);

        return databaseWord is null ? null : MapToDomainWord(databaseWord);
    }
    public async Task<DomainWord?> AddWordAsync(DomainWord word, CancellationToken cancellationToken = default)
    {
        int? categoryId =
            await GetCategoryIdAsync(word.Type,cancellationToken);

        bool alreadyExists =
            await _context.Words
                .AnyAsync(
                    existingWord =>
                        existingWord.Value == word.Text
                        && existingWord.CategoryId == categoryId,
                    cancellationToken);

        if (alreadyExists)
        {
            throw new InvalidOperationException(
                $"The word '{word.Text}' already exists.");
        }

        var databaseWord = new EfWord
        {
            Value = word.Text,
            CategoryId = categoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Words.Add(databaseWord);

        await _context.SaveChangesAsync(cancellationToken);
        word.Id = databaseWord.Id;
        word.WordLetterCount = LetterCounter.CountLetters(word.Text);
        return word;
    }

    public async Task<bool> DeleteWordByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        EfWord? databaseWord = await _context.Words.FindAsync([id], cancellationToken);

        if (databaseWord is null)
        {
            return false;
        }

        _context.Words.Remove(databaseWord);
        await _context.SaveChangesAsync(
            cancellationToken);
        return true;
    }
    private static DomainWord MapToDomainWord(
        EfWord databaseWord)
    {
        return new DomainWord
        {
            Id = databaseWord.Id,
            Text = databaseWord.Value,
            Type = MapCategoryToWordType(
                databaseWord.Category?.Name),
            WordLetterCount =LetterCounter.CountLetters(databaseWord.Value)};
    }
    private async Task<int?> GetCategoryIdAsync(string wordType, CancellationToken cancellationToken)
    {
        string categoryName = MapWordTypeToCategory(wordType);
        return await _context.Categories
            .Where(category =>
                category.Name == categoryName)
            .Select(category =>
                (int?)category.Id)
            .FirstOrDefaultAsync(
                cancellationToken);
    }
    private static string MapCategoryToWordType(string? categoryName)
    {
        return categoryName switch
        {
            "Adjective" => SupportedWordTypes.Adjective,
            "Noun" => SupportedWordTypes.Noun,
            "Verb" => SupportedWordTypes.Verb,
            _ => string.Empty
        };
    }
    private static string MapWordTypeToCategory(string wordType)
    {
        return wordType switch
        {
            SupportedWordTypes.Adjective =>"Adjective",
            SupportedWordTypes.Noun => "Noun",
            SupportedWordTypes.Verb => "Verb",
            _ => throw new InvalidOperationException($"Unsupported word type '{wordType}'.")
        };
    }
}