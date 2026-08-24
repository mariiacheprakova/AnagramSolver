using AnagramSolver.Contracts;
namespace AgentDemo;

public class NoOpSearchLogRepository : ISearchLogRepository
{
    public Task AddAsync(int resultCount, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
