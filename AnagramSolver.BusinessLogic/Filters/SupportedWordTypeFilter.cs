using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic.Filters;

public class SupportedWordTypeFilter : WordFilterBase
{
    private static readonly HashSet<string> supportedTypes =
    [
        "bdv",
        "dkt",
        "vksm"
    ];

    public override bool Handle(Word word, Dictionary<char, int> availableLetters)
    {
        if(!supportedTypes.Contains(word.Type))
        {
            return false;
        }
        
        return base.Handle(word, availableLetters);
    }
   

}
