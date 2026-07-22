namespace AnagramSolver.BusinessLogic;

public class MemoryCache<T>
{
    private Dictionary<string, T> _cache = new();

    public void Set(string key, T value) => _cache[key] = value;
    public bool TryGet(string key, out T value) => _cache.TryGetValue(key, out value!);

}

