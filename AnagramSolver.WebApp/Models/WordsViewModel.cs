using AnagramSolver.Contracts.Models;

namespace AnagramSolver.WebApp.Models;
public class WordsViewModel
{
    public IReadOnlyCollection<Word> Words { get; set; } = Array.Empty<Word>();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}
