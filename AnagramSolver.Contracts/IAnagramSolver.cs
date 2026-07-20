public interface IAnagramSolver
{
    Task<IReadOnlyCollection<string>> GetAnagramsAsync(Dictionary<char, int> userInputDictionary,CancellationToken cancellationToken = default);
}