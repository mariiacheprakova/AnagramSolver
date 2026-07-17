using AnagramSolver.Contracts.Models;

public interface IAnagramSolver
{
    HashSet<string> GetAnagrams(Dictionary<char, int> userInputDictionary);
}