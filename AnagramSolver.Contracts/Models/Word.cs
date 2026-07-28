namespace AnagramSolver.Contracts.Models;

public class Word
{
    public string Text { get; set; }
    public string Type { get; set; }
    public int Id { get; set; }
    public Dictionary<char, int> WordLetterCount { get; set; }
}
