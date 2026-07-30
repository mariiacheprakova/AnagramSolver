using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;

public class WordFileParser
{
    public static Word[] ParseWords(IList<string> lines)
    {
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
            {
                continue;
            }

            var word = new Word
            {
                Text = wordText,
                Type = wordType,
                Id = count + 1,
                WordLetterCount = CountLetters(wordText),
            };

            temporaryWords[count] = word;
            count++;
        }

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
