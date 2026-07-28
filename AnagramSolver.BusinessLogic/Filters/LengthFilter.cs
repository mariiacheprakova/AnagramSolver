using AnagramSolver.Contracts.Models;
namespace AnagramSolver.BusinessLogic.Filters;

public class LengthFilter : WordFilterBase
{
    public override bool Handle(Word word, Dictionary<char, int> availableLetters)
    {
        int availableLetterCount = availableLetters.Values.Sum();
        if(word.Text.Length > availableLetterCount)
        {
            return false;
        }

        return base.Handle(word,availableLetters);

    }
}
