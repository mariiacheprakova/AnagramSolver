using AnagramSolver.Contracts.Models;

public interface IWordRepository
{
    Task<Word[]> GetAllWordsAsync(CancellationToken cancellationToken = default);
}