using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using BusinessWord = AnagramSolver.Contracts.Models.Word;

namespace AnagramSolver.EF.CodeFirst.Repositories;

public class EfWordRepository : IWordRepository
{
    private readonly AnagramDbContext _context;
    private readonly LetterCounter _letterCounter;

    public EfWordRepository(
        AnagramDbContext context,
        LetterCounter letterCounter)
    {
        _context = context;
        _letterCounter = letterCounter;
    }

    public async Task<IEnumerable<BusinessWord>>
        GetAllWordsAsync()
    {
        var databaseWords = await _context.Words
            .AsNoTracking()
            .Include(word => word.Category)
            .ToListAsync();

        return databaseWords.Select(word =>
            new BusinessWord
            {
                Id = word.Id,
                Text = word.Value,
                Type = word.Category?.Name ?? "No category",
                WordLetterCount =
                    _letterCounter.CountLetters(word.Value)
            });
    }
}