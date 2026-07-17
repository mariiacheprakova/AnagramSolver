using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;

public class AnagramSolverService : IAnagramSolver
{
    private readonly IWordRepository _wordRepository;

    public AnagramSolverService(IWordRepository wordRepository) // constructor 
    {
        _wordRepository = wordRepository;
    }

    public HashSet<string> GetAnagrams(
        Dictionary<char, int> userInputDictionary)
    {
        Word[] loadedWords =
            _wordRepository.GetAllWords();

        Word[] allWords = GetSupportedWords(loadedWords); //pick only adj verbs and nouns

        var results = new HashSet<string>();

        var threeWordAnagrams = FindThreeWordAnagrams(userInputDictionary, allWords);
        var twoWordAnagrams = FindTwoWordAnagrams(userInputDictionary, allWords);
        var oneWordAnagrams = FindOneWordAnagrams(userInputDictionary, allWords);

        results.UnionWith(threeWordAnagrams);
        results.UnionWith(twoWordAnagrams);
        results.UnionWith(threeWordAnagrams);

        return results;
    }

    private Word[] GetSupportedWords(Word[] loadedWords)
    {
        var temporaryWords =
            new Word[loadedWords.Length];

        var count = 0;

        var seenWords =
            new HashSet<string>();

        foreach (Word word in loadedWords)
        {
            bool hasSupportedType =
                word.Type == "bdv" ||
                word.Type == "dkt" ||
                word.Type == "vksm";

            if (!hasSupportedType)
            {
                continue;
            }

            string key = $"{word.Text}|{word.Type}";

            if (!seenWords.Add(key))
            {
                continue;
            }

            temporaryWords[count] = word;
            count++;
        }

        Word[] result = new Word[count];

        Array.Copy(
            temporaryWords,
            result,
            count);

        return result;
    }

    private HashSet<string> FindOneWordAnagrams(
        Dictionary<char, int> inputLetters,
        Word[] allWords)
    {
        var oneWordAnagrams = new HashSet<string>();
        foreach (Word word in allWords)
        {
            if (DictionariesAreEqual(
                inputLetters,
                word.WordLetterCount))
            {
                oneWordAnagrams.Add(word.Text);
            }
        }
        return oneWordAnagrams;
    }

    private HashSet<string> FindTwoWordAnagrams(
        Dictionary<char, int> inputLetters,
        Word[] allWords)
    {
        var twoWordAnagrams = new HashSet<string>();
        foreach (Word firstWord in allWords)
        {
            if (!CanUseWord(
                inputLetters,
                firstWord.WordLetterCount))
            {
                continue;
            }

            Dictionary<char, int> afterFirstWord =
                SubtractLetters(
                    inputLetters,
                    firstWord.WordLetterCount);

            foreach (Word secondWord in allWords)
            {
                if (!CanUseWord(
                    afterFirstWord,
                    secondWord.WordLetterCount))
                {
                    continue;
                }

                Dictionary<char, int> afterSecondWord =
                    SubtractLetters(
                        afterFirstWord,
                        secondWord.WordLetterCount);

                if (afterSecondWord.Count != 0)
                {
                    continue;
                }

                string formattedResult =
                    FormatTwoWords(firstWord, secondWord);

                twoWordAnagrams.Add(formattedResult);
            }
        }
        return twoWordAnagrams;
    }

    private HashSet<string> FindThreeWordAnagrams(
        Dictionary<char, int> inputLetters,
        Word[] allWords)
    {
        var threeWordAnagrams = new HashSet<string>();
        foreach (Word firstWord in allWords)
        {
            if (!CanUseWord(
                inputLetters,
                firstWord.WordLetterCount))
            {
                continue;
            }

            Dictionary<char, int> afterFirstWord =
                SubtractLetters(
                    inputLetters,
                    firstWord.WordLetterCount);

            foreach (Word secondWord in allWords)
            {
                if (!CanUseWord(
                    afterFirstWord,
                    secondWord.WordLetterCount))
                {
                    continue;
                }

                Dictionary<char, int> afterSecondWord =
                    SubtractLetters(
                        afterFirstWord,
                        secondWord.WordLetterCount);

                foreach (Word thirdWord in allWords)
                {
                    if (!CanUseWord(
                        afterSecondWord,
                        thirdWord.WordLetterCount))
                    {
                        continue;
                    }

                    Dictionary<char, int> afterThirdWord =
                        SubtractLetters(
                            afterSecondWord,
                            thirdWord.WordLetterCount);

                    if (afterThirdWord.Count != 0)
                    {
                        continue;
                    }

                    string formattedResult;

                    bool hasCorrectTypes =
                        TryFormatThreeWords(
                            firstWord,
                            secondWord,
                            thirdWord,
                            out formattedResult);

                    if (!hasCorrectTypes)
                    {
                        continue;
                    }

                    threeWordAnagrams.Add(formattedResult);
                }
            }
        }
        return threeWordAnagrams;
    }

    private bool CanUseWord(
        Dictionary<char, int> availableLetters,
        Dictionary<char, int> requiredLetters)
    {
        foreach (
            KeyValuePair<char, int> requiredLetter
            in requiredLetters)
        {
            int availableCount;

            bool letterExists =
                availableLetters.TryGetValue(
                    requiredLetter.Key,
                    out availableCount);

            if (!letterExists)
            {
                return false;
            }

            if (availableCount < requiredLetter.Value)
            {
                return false;
            }
        }

        return true;
    }

    private Dictionary<char, int> SubtractLetters(
        Dictionary<char, int> availableLetters,
        Dictionary<char, int> usedLetters)
    {
        var remainingLetters =
            new Dictionary<char, int>(availableLetters);

        foreach (
            KeyValuePair<char, int> usedLetter
            in usedLetters)
        {
            remainingLetters[usedLetter.Key] -=
                usedLetter.Value;

            if (remainingLetters[usedLetter.Key] == 0)
            {
                remainingLetters.Remove(
                    usedLetter.Key);
            }
        }

        return remainingLetters;
    }

    private bool DictionariesAreEqual(
        Dictionary<char, int> first,
        Dictionary<char, int> second)
    {
        if (first.Count != second.Count)
        {
            return false;
        }

        foreach (
            KeyValuePair<char, int> pair
            in first)
        {
            int secondValue;

            bool letterExists =
                second.TryGetValue(
                    pair.Key,
                    out secondValue);

            if (!letterExists)
            {
                return false;
            }

            if (secondValue != pair.Value)
            {
                return false;
            }
        }

        return true;
    }

    private string FormatTwoWords(
        Word firstWord,
        Word secondWord)
    {
        Word? adjective = null;
        Word? noun = null;
        Word? verb = null;

        Word[] words =
        {
            firstWord,
            secondWord
        };

        foreach (Word word in words)
        {
            if (word.Type == "bdv")
            {
                adjective = word;
            }
            else if (word.Type == "dkt")
            {
                noun = word;
            }
            else if (word.Type == "vksm")
            {
                verb = word;
            }
        }

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

    private bool TryFormatThreeWords(
        Word firstWord,
        Word secondWord,
        Word thirdWord,
        out string formattedResult)
    {
        Word? adjective = null;
        Word? noun = null;
        Word? verb = null;

        Word[] words =
        {
            firstWord,
            secondWord,
            thirdWord
        };

        foreach (Word word in words)
        {
            if (word.Type == "bdv")
            {
                if (adjective != null)
                {
                    formattedResult = string.Empty;
                    return false;
                }

                adjective = word;
            }
            else if (word.Type == "dkt")
            {
                if (noun != null)
                {
                    formattedResult = string.Empty;
                    return false;
                }

                noun = word;
            }
            else if (word.Type == "vksm")
            {
                if (verb != null)
                {
                    formattedResult = string.Empty;
                    return false;
                }

                verb = word;
            }
        }

        if (adjective == null ||
            noun == null ||
            verb == null)
        {
            formattedResult = string.Empty;
            return false;
        }

        formattedResult =
            $"{adjective.Text} {noun.Text} {verb.Text}";

        return true;
    }
}