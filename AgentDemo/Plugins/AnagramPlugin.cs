using Microsoft.SemanticKernel;
using System.ComponentModel;
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;

namespace AgentDemo.Plugins;

public class AnagramPlugin(IAnagramSolver anagramSolver, FormatingUserInput formatingUserInput)
{
    private readonly IAnagramSolver _anagramSolver = anagramSolver;
    private readonly FormatingUserInput _formatingUserInput = formatingUserInput;
  
    [KernelFunction]
    [Description("Finds anagrams from a text file")]
    public async Task<IReadOnlyCollection<string>> FindAnagrams(string input)
    {
        Console.WriteLine($"[PLUGIN] FindAnagrams called with {input}.");
        var userInputDictionary = _formatingUserInput.CountLetters(input);
        var foundAnagrams = await _anagramSolver.GetAnagramsAsync(input, userInputDictionary);
        return foundAnagrams;
    }

    [KernelFunction]
    [Description("Counts the number of anagrams for the provided word.")]    
    public async Task<int> NumberOfFoundAnagramsAsync(string input)
    {
        Console.WriteLine($"[PLUGIN] NumberOfFoundAnagrams got called with {input}");
        var foundAnagrams = await FindAnagrams(input);

        return foundAnagrams.Count;
    }
}

