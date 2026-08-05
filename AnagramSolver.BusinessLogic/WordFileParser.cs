using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;

public static class WordFileParser
{
    public static Word[] ParseWords(IList<string> lines)
    {
        var words = lines
        .Select(line =>
        {
            var parts = line.Split();
            return new Word
            {
                Text = parts[0],
                Type = parts[1]
            };
        })
        .DistinctBy(word => (word.Text, word.Type))
        .Select((word, index) => new Word
        {
            Id = index + 1,
            Text = word.Text,
            Type = word.Type,
            WordLetterCount = LetterCounter.CountLetters(word.Text)
        })
        .ToArray();
        return words;
    }
}

