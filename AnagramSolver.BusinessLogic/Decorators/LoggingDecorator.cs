using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic.Decorators;

public class LoggingDecorator : IAnagramSolver
{
    private readonly ILogger _logger;
    private readonly IAnagramSolver _inner;

    public LoggingDecorator(ILogger logger, IAnagramSolver inner)
    {
        _logger = logger;
        _inner = inner;
    }
    public async Task<IReadOnlyCollection<string>> GetAnagramsAsync(Dictionary<char, int> userInputDictionary, CancellationToken cancellationToken = default)
    {
        _logger.Log("Searching for anagrams...");
        IReadOnlyCollection<string> result = await _inner.GetAnagramsAsync(userInputDictionary, cancellationToken);
        _logger.Log($"Found {result.Count} anagrams");
        return result;
    }
}
