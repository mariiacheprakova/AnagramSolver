using AnagramSolver.Contracts.Models;

public interface IWordRepository
{
    IList<Word> GetAllWords(string fileName);
}