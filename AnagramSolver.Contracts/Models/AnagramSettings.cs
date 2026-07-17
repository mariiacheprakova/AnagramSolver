namespace AnagramSolver.Contracts.Models
{
    public class AnagramSettings
    {
        public int MaxAnagramsCount { get; set; }
        public int MinimumWordLength { get; set; }
        public string TextFileName { get; set; } = string.Empty;


    }
    public class Word
    {
        public string Text { get; set; } //= string.Empty;
        public string Type { get; set; }
        public Dictionary<char, int> WordLetterCount { get; set; }
    }
}
