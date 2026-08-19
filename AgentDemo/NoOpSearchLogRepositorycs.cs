using AnagramSolver.Contracts;
namespace AgentDemo;

public class NoOpSearchLogRepository : ISearchLogRepository
{
    public Task AddAsync(string searchText, int resultCount, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
