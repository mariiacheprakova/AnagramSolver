using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;

public class AnagramSolverService : IAnagramSolver
{
    //private readonly IWordRepository _wordRepository;
    //public AnagramSolverService(IWordRepository wordRepository)
    //{
    //     _wordRepository = wordRepository;
    //}


    private readonly IWordRepository _wordRepository;

    public AnagramSolverService(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public IList<string> GetAnagrams(Dictionary<char, int> userInputDictionary)
    {
        var lines = _wordRepository.GetAllWords("zodynas.txt");
        List<Word> adjectives = lines.Where(word => word.Type == "bdv").ToList();
        List<Word> nouns = lines.Where(word => word.Type == "dkt").ToList();
        List<Word> verbs = lines.Where(word => word.Type == "vksm").ToList();
        HashSet<string> allAnagrams = new HashSet<string>();

        List<Word> allWords = adjectives.Concat(nouns).Concat(verbs).GroupBy(word => new { word.Text, word.Type }).Select(group => group.First()).ToList();

        FindOneWordAnagrams(userInputDictionary, allWords, allAnagrams);

        FindTwoWordAnagrams(userInputDictionary, allWords, allAnagrams);

        FindThreeWordAnagrams(userInputDictionary, allWords, allAnagrams);


        return allAnagrams.ToList();
    }

    private void FindOneWordAnagrams(Dictionary<char, int> inputLetters, List<Word> allWords, HashSet<string> results)
    {
        foreach (Word word in allWords)
        {
            if (DictionariesAreEqual(inputLetters, word.WordLetterCount))
            {
                results.Add(word.Text);
            }
        }
    }

    private void FindTwoWordAnagrams(Dictionary<char, int> inputLetters, List<Word> allWords, HashSet<string> results)
    {
        foreach (Word firstWord in allWords)
        {
            if (!CanUseWord(inputLetters, firstWord.WordLetterCount))
            {
                continue;
            }

            Dictionary<char, int> afterFirstWord = SubtractLetters(inputLetters, firstWord.WordLetterCount);

            foreach (Word secondWord in allWords)
            {
                if (!CanUseWord(afterFirstWord, secondWord.WordLetterCount))
                {
                    continue;
                }

                Dictionary<char, int> afterSecondWord = SubtractLetters(afterFirstWord, secondWord.WordLetterCount);

                if (afterSecondWord.Count != 0)
                {
                    continue;
                }

                string formattedResult = FormatTwoWords(firstWord, secondWord);

                results.Add(formattedResult);
            }
        }
    }

    private void FindThreeWordAnagrams(Dictionary<char, int> inputLetters, List<Word> allWords, HashSet<string> results)
    {
        foreach (Word firstWord in allWords)
        {
            if (!CanUseWord(inputLetters, firstWord.WordLetterCount))
            {
                continue;
            }

            Dictionary<char, int> afterFirstWord = SubtractLetters(inputLetters, firstWord.WordLetterCount);

            foreach (Word secondWord in allWords)
            {
                if (!CanUseWord(afterFirstWord, secondWord.WordLetterCount))
                {
                    continue;
                }

                Dictionary<char, int> afterSecondWord = SubtractLetters(afterFirstWord, secondWord.WordLetterCount);

                foreach (Word thirdWord in allWords)
                {
                    if (!CanUseWord(afterSecondWord, thirdWord.WordLetterCount))
                    {
                        continue;
                    }

                    Dictionary<char, int> afterThirdWord = SubtractLetters(afterSecondWord, thirdWord.WordLetterCount);

                    if (afterThirdWord.Count != 0)
                    {
                        continue;
                    }

                    Word[] words = { firstWord, secondWord, thirdWord };

                    bool hasCorrectTypes =
                        words.Count(word => word.Type == "bdv") == 1 &&
                        words.Count(word => word.Type == "dkt") == 1 &&
                        words.Count(word => word.Type == "vksm") == 1;

                    if (!hasCorrectTypes)
                    {
                        continue;
                    }

                    Word adjective =
                        words.Single(word => word.Type == "bdv");

                    Word noun =
                        words.Single(word => word.Type == "dkt");

                    Word verb =
                        words.Single(word => word.Type == "vksm");

                    results.Add(
                        $"{adjective.Text} {noun.Text} {verb.Text}");
                }
            }
        }
    }

    private bool CanUseWord(Dictionary<char, int> availableLetters, Dictionary<char, int> requiredLetters)
    {
        return requiredLetters.All(requiredLetter => availableLetters.TryGetValue(requiredLetter.Key, out int availableCount) && availableCount >= requiredLetter.Value);
    }

    private Dictionary<char, int> SubtractLetters(Dictionary<char, int> availableLetters, Dictionary<char, int> usedLetters)
    {
        Dictionary<char, int> remainingLetters = new Dictionary<char, int>(availableLetters);

        foreach (var usedLetter in usedLetters)
        {
            remainingLetters[usedLetter.Key] -= usedLetter.Value;

            if (remainingLetters[usedLetter.Key] == 0)
            {
                remainingLetters.Remove(usedLetter.Key);
            }
        }

        return remainingLetters;
    }

    static bool DictionariesAreEqual(Dictionary<char, int> first, Dictionary<char, int> second)
    {
        return first.Count == second.Count && first.All(pair => second.TryGetValue(pair.Key, out int secondValue) && secondValue == pair.Value);
    }

    static string FormatTwoWords(Word firstWord, Word secondWord)
    {
        Word[] words = { firstWord, secondWord };

        Word? adjective = words.FirstOrDefault(word => word.Type == "bdv");

        Word? noun = words.FirstOrDefault(word => word.Type == "dkt");

        Word? verb = words.FirstOrDefault(word => word.Type == "vksm");

        if (adjective != null && noun != null)
        {
            return $"{adjective.Text} {noun.Text}";
        }

        if (noun != null && verb != null)
        {
            return $"{noun.Text} {verb.Text}";
        }

        if (adjective != null && verb != null)
        {
            return $"{adjective.Text} {verb.Text}";
        }

        return $"{firstWord.Text} {secondWord.Text}";
    }
}