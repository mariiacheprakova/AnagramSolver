namespace AnagramSolver.EF.CodeFirst.Models;
public class SearchLog
{
    public int Id { get; set; }
    public string SearchText { get; set; } = string.Empty;
    public int ResultCount { get; set; }
    public DateTime SearchedAt { get; set; } = DateTime.UtcNow;
}
