using AnagramSolver.Contracts.Models;

public interface IAnagramSolver
{
    IList<string> GetAnagrams(Dictionary<char, int> userInputDictionary);
}