using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic;

public static class WordFileParser
{
    public static Word[] ParseWords(IList<string> lines)
    {
        Word[] words = lines
            .Select((line, lineIndex) =>
            {
                string[] parts = line.Split(
                    (char[]?)null,
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries);

                if (parts.Length < 2)
                {
                    throw new FormatException(
                        $"Invalid dictionary line {lineIndex + 1}: '{line}'. " +
                        "Expected a word and a word type.");
                }

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
                WordLetterCount =
                    LetterCounter.CountLetters(word.Text)
            })
            .ToArray();

        return words;
    }
}