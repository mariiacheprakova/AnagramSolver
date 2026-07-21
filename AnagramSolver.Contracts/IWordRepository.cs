using AnagramSolver.Contracts.Models;

public interface IWordRepository
{
    Task<Word[]> GetAllWordsAsync(CancellationToken cancellationToken = default);
    Task<Word?> GetWordByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Word?> AddWordAsync(Word word, CancellationToken cancellationToken = default);
    Task<bool> DeleteWordByIdAsync(int id, CancellationToken cancellationToken = default);
}