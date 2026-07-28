using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Contracts;

public interface IWordFilter
{
    IWordFilter SetNext(IWordFilter next);
    bool Handle(Word word, Dictionary<char, int> avalaibleLetters);
}
