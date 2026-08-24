using AnagramSolver.Contracts;

namespace AnagramSolver.AnagramMcp.Services;

public class NoOpSearchLogRepository :ISearchLogRepository
{
    public Task AddAsync(int resultCount, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
