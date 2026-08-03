using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;

public class EfWordRepository : IWordRepository
{
    private readonly AnagramDbContext _context;

    public EfWordRepository(AnagramDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Word>> GetAllWordsAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Words
            .ToListAsync(cancellationToken);
    }
}