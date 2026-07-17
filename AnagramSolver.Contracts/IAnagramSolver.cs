public interface IAnagramSolver
{
    IReadOnlyCollection<string> GetAnagrams(Dictionary<char, int> userInputDictionary);
}