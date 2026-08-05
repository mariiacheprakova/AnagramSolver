namespace AnagramSolver.Contracts;
public interface IAnagramSolver
{
    Task<IReadOnlyCollection<string>> GetAnagramsAsync(
        string searchText,
        Dictionary<char, int> userInputDictionary,
        CancellationToken cancellationToken = default);
}
