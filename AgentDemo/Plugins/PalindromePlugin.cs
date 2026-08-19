using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AgentDemo.Plugins;

public class PalindromePlugin
{
    [KernelFunction]
    [Description("Checks whether found anagram is a palindrome(reads the same backwards and forwards")]
    public bool IsPalindromne(string input)
    {
        Console.WriteLine($"[PLUGIN] IsPalindrome was called with {input}");
        var normalized = input.Trim().ToLower();

        return normalized == new string(normalized.Reverse().ToArray());
    }
}
