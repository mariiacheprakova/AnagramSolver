using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;

public static class WordFileParser
{
    public static Word[] ParseWords(
        IList<string> lines)
    {
        var seenWords =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        return lines
            .Select(line => line.Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries
                | StringSplitOptions.TrimEntries))
            .Where(parts => parts.Length >= 2)
            .Select(parts =>
            {
                string wordText = parts[0];
                string wordType = parts[1];

                return new
                {
                    WordText = wordText,
                    WordType = wordType,
                    Key = $"{wordText}|{wordType}"
                };
            })
            .Where(parsedWord =>
                seenWords.Add(parsedWord.Key))
            .Select((parsedWord, index) =>
                new Word
                {
                    Id = index + 1,
                    Text = parsedWord.WordText,
                    Type = parsedWord.WordType,
                    WordLetterCount =
                        LetterCounter.CountLetters(
                            parsedWord.WordText)
                })
            .ToArray();
    }
}