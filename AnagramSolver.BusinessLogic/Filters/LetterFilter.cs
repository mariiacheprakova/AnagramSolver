using AnagramSolver.Contracts.Models;
namespace AnagramSolver.BusinessLogic.Filters;

public class LetterFilter : WordFilterBase
{
    public override bool Handle(Word word, Dictionary<char, int> availableLetters)
    {
        foreach(KeyValuePair<char,int> requiredLetter in word.WordLetterCount)
        {
            bool exists = availableLetters.TryGetValue(requiredLetter.Key, out int availableCount);
            if(!exists)
            {
                return false;
            }
            if(availableCount < requiredLetter.Value)
            {
                return false;
            }
        }
        return base.Handle(word, availableLetters);
    }
}
