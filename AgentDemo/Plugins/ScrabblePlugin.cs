using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AgentDemo.Plugins;

public class ScrabblePlugin
{
    private static readonly Dictionary<char, int> LetterValues = new()
    {
        ['a'] = 1,
        ['e'] = 1,
        ['i'] = 1,
        ['o'] = 1,
        ['u'] = 1,
        ['l'] = 1,
        ['n'] = 1,
        ['s'] = 1,
        ['t'] = 1,
        ['r'] = 1,
        ['ą'] = 2,
        ['ū'] = 2,
        ['b'] = 3,
        ['c'] = 3,
        ['į'] = 3,
        ['p'] = 3,
        ['ė'] = 4,
        ['ų'] = 4,
        ['v'] = 4,
        ['j'] = 4,
        ['y'] = 4,
        ['ž'] = 5,
        ['j'] = 8,
        ['š'] = 8,
        ['č'] = 10,
        ['z'] = 10,
    };
    [KernelFunction]
    [Description("Calculates a Scrabble-style point score of an anagram using letter values,Lithuanian diacritic letters default to 1 point.")]
    public int GetScrabbleScore(string input)
    {
        Console.WriteLine($"[PLUGIN] GetScrabbleScore was called with {input}");
        var normalised = input.Trim().ToLower();
        return normalised.Sum(letter => LetterValues.GetValueOrDefault(letter, 1));
        //return 9999;
    }
}
