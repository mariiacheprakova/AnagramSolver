namespace AnagramSolver.WebApp.Models;
public class AnagramViewModel
{
    public string? Input { get; set; }
    public IReadOnlyCollection<string> Anagrams { get; set; } = Array.Empty<string>();
}
