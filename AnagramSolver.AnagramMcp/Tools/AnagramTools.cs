using AnagramSolver.Contracts;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AnagramSolver.AnagramMcp.Tools;

[McpServerToolType]
public static class AnagramTools
{
    [McpServerTool]
    [Description("Finds all anagrams (single word and multiword combinations) that can be formed from the letters of the given text.")]
    public static async Task<IReadOnlyCollection<string>> FindAnagrams(IAnagramSolver anagramSolver, [Description("The word or phrase to find anagrams for")] string text, CancellationToken cancellationToken)
    {
        Dictionary<char, int> userInputDictionary = LetterCounter.CountLetters(text);
        return await anagramSolver.GetAnagramsAsync(text, userInputDictionary, cancellationToken);
    }

    [McpServerTool]
    [Description("Counts how many anagrams can be formed from the letters of the given text.")]
    public static async Task<int> CountAnagrams(IAnagramSolver anagramSolver, [Description("The word or phrase to count anagrams for")] string text, CancellationToken cancellationToken)
    {
        var foundAnagrams = await FindAnagrams(anagramSolver, text, cancellationToken);
        return foundAnagrams.Count;
    }
}
