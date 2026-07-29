using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;

public class WordFileParser
{
    public static Word[] ParseWords(IList<string> lines)
    {
<<<<<<< HEAD
        var letterCounter = new LetterCounter();
        var seenWords = new HashSet<string>();

        var temporaryWords = lines.Select(line =>
=======
        var temporaryWords = new Word[lines.Count];
        var count = 0;

        var seenWords = new HashSet<string>();

        foreach (string line in lines)
        {
            var parts = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

            var wordText = parts[0];
            var wordType = parts[1];

            var key = $"{wordText}|{wordType}";

            if (!seenWords.Add(key))
>>>>>>> origin/feature/AnagramSolver
            {
                var parts = line.Split();

<<<<<<< HEAD
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
=======
            var word = new Word
            {
                Text = wordText,
                Type = wordType,
                Id = count + 1,
                WordLetterCount = CountLetters(wordText),
            };
>>>>>>> origin/feature/AnagramSolver

        return temporaryWords;

<<<<<<< HEAD
    }
}
      
=======
        var result = new Word[count];

        Array.Copy(temporaryWords, result, count);

        return result;
    }

    public static Dictionary<char, int> CountLetters(string text)
    {
        var letterCount = new Dictionary<char, int>();

        foreach (char character in text)
        {
            if (letterCount.ContainsKey(character))
            {
                letterCount[character]++;
            }
            else
            {
                letterCount[character] = 1;
            }
        }

        return letterCount;
    }
}
>>>>>>> origin/feature/AnagramSolver
