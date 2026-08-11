namespace AnagramSolver.Contracts;
public interface ISearchLogRepository
{
    Task AddAsync(int resultCount, CancellationToken cancellationToken = default);
}