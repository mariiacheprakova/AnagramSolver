using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic.Filters;
public class SupportedWordTypeFilter : WordFilterBase
{
    public override bool Handle(Word word, Dictionary<char, int> availableLetters)
    {
        bool isSupportedType =
            word.Type == SupportedWordTypes.Adjective
            || word.Type == SupportedWordTypes.Noun
            || word.Type == SupportedWordTypes.Verb;

        if (!isSupportedType)
        {
            return false;
        }
        return base.Handle(word, availableLetters);
    }
}
