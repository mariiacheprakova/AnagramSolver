namespace AnagramSolver.BusinessLogic.Decorators;

public class CacheDecorator : IAnagramSolver
{
    private readonly IAnagramSolver _inner;
    private readonly MemoryCache<IReadOnlyCollection<string>> _cache;


    public CacheDecorator(IAnagramSolver inner, MemoryCache<IReadOnlyCollection<string>> cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<IReadOnlyCollection<string>> GetAnagramsAsync(Dictionary<char, int> userInputDictionary
        , CancellationToken cancellationToken)
    {
        string key = CreateCacheKey(userInputDictionary);
        if (_cache.TryGet(key, out IReadOnlyCollection<string> cachedResult))
        {
            return cachedResult;
        }

        IReadOnlyCollection<string> results = await _inner.GetAnagramsAsync(userInputDictionary, cancellationToken);
        _cache.Set(key, results);

        return results;
    }

    private static string CreateCacheKey(Dictionary<char, int> userInputDictionary)
    {
        char[] letters = userInputDictionary.Keys.ToArray();
        Array.Sort(letters);
        string key = string.Empty;

        foreach (char letter in letters)
        {
            int count = userInputDictionary[letter];
            key += $"{letter}:{count}|";
        }
        return key;
    }

}