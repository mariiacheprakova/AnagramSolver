using AnagramSolver.Contracts.Models;

public interface IWordRepository
{
    Word[] GetAllWords(string fileName);
}