using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AgentDemo.Plugins;
public class TextPlugin
{
    [KernelFunction]
    [Description("Counts the number of characters")]
    public int CountCharacters(string input)
    {
        Console.WriteLine($"[PLUGIN] CountCharacters was called with {input}");
        var lengthOfInput = input.Length;
        Console.WriteLine($"There are {lengthOfInput} chars in input");

        return lengthOfInput;
    }

    [KernelFunction]
    [Description("Transforms input to upper case.")]
    public string TransformToUpperCase(string input)
    {
        Console.WriteLine($"[PLUGIN] TransformToUpperCase called with {input}");

        return input.ToUpper();
    }
}
