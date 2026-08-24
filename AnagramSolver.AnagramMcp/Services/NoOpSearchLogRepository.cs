using AnagramSolver.Contracts;

namespace AnagramSolver.AnagramMcp.Services;

public class NoOpSearchLogRepository :ISearchLogRepository
{
    public Task AddAsync(string searchText, int resultCount,CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
