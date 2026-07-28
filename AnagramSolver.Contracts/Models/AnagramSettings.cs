namespace AnagramSolver.Contracts.Models
{
    public class AnagramSettings
    {
        public int MaxAnagramsCount { get; set; }
        public int MinimumWordLength { get; set; }
        public string TextFileName { get; set; } = string.Empty;

    }
}
