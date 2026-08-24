namespace AnagramSolver.Contracts;
public interface ISearchLogRepository
{
    Task AddAsync(string searchText,int resultCount,CancellationToken cancellationToken = default);
}