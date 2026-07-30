namespace AnagramSolver.Contracts.Models;

public class Word
{
    public string? Text { get; init; }
    public string? Type { get; init; }
    public int Id { get; set; }
    public Dictionary<char, int>? WordLetterCount { get; set; } = new();
}
