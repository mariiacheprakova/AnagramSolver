using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;
public class WordFileParser
{
    public static Word[] ParseWords(IList<string> lines)
    {

        var letterCounter = new LetterCounter();
        var seenWords = new HashSet<string>();

        var temporaryWords = lines.Select(line =>
            {
                var parts = line.Split();

                var wordText = parts[0];
                var wordType = parts[1];
                return (
                wordText,
                wordType,
                key: $"{wordText}| {wordType}"
                );

            })
            .Where(parts => seenWords.Add(parts.key))
            .Select((parts, index) => new Word
            {
                Text = parts.wordText,
                Type = parts.wordType,
                Id = index + 1,
                WordLetterCount = LetterCounter.CountLetters(parts.wordText)
            })
            .ToArray();

        return temporaryWords;
    }
}
