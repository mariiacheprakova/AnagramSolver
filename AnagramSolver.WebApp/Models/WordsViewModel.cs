namespace AnagramSolver.WebApp.Models;

    public class WordsViewModel
    {
    public IReadOnlyCollection<string> Words { get; set; } = Array.Empty<string>();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    //The value of this property is produced by the expression on the right.
}

