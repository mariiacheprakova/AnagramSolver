
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;


namespace AnagramSolver.BusinessLogic.Filters;

public class WordFilterBase : IWordFilter
{
    private IWordFilter? _next;
    public IWordFilter SetNext(IWordFilter next)
    {
        _next = next;
        return next;
    }

   public virtual bool Handle(Word word,Dictionary<char,int> availableLetters)
    {
        if(_next is null)
        {
            return true;
        }
        return _next.Handle(word, availableLetters);
    }
}
