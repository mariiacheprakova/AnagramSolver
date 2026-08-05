using AnagramSolver.Contracts;
using AnagramSolver.EF.CodeFirst.Data;
using AnagramSolver.EF.CodeFirst.Models;

namespace AnagramSolver.EF.CodeFirst.Repositories;
public class EfSearchLogRepository : ISearchLogRepository
{
    private readonly AnagramDbContext _context;
    public EfSearchLogRepository(
        AnagramDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(
        string searchText,
        int resultCount,
        CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrWhiteSpace(searchText))
        {
            throw new ArgumentException(
                "Search text cannot be empty.",
                nameof(searchText));
        }

        if(resultCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(resultCount));
        }

        var searchLog = new SearchLog
        {
            SearchText = searchText,
            ResultCount = resultCount,
            SearchedAt = DateTime.UtcNow
        };

        _context.SearchLogs.Add(searchLog);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}