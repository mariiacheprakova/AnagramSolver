using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;
public class AnagramSolverService : IAnagramSolver
{
    private readonly IWordRepository _wordRepository;
    private readonly IWordFilter _filterChain;
    private readonly ISearchLogRepository _searchLogRepository;
    public AnagramSolverService(
        IWordRepository wordRepository,
        IWordFilter filterChain,
        ISearchLogRepository searchLogRepository)
    {
        _wordRepository = wordRepository;
        _filterChain = filterChain;
        _searchLogRepository = searchLogRepository;
    }
    public async Task<IReadOnlyCollection<string>> GetAnagramsAsync(
        Dictionary<char, int> userInputDictionary,
        CancellationToken cancellationToken = default)
    {
        Word[] loadedWords =
            await _wordRepository.GetAllWordsAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        Word[] allWords =
            GetSupportedWords(
                loadedWords,
                userInputDictionary);

        return FindOneWordAnagrams(
                userInputDictionary,
                allWords)
            .Union(
                FindTwoWordAnagrams(
                    userInputDictionary,
                    allWords))
            .Union(
                FindThreeWordAnagrams(
                    userInputDictionary,
                    allWords))
            .ToHashSet();
    }
    private Word[] GetSupportedWords(
        Word[] loadedWords,
        Dictionary<char, int> userInputDictionary)
    {
        return loadedWords
            .Where(word =>
                _filterChain.Handle(
                    word,
                    userInputDictionary))
            .DistinctBy(word =>
                $"{word.Text}|{word.Type}")
            .ToArray();
    }
    private HashSet<string> FindOneWordAnagrams(
        Dictionary<char, int> inputLetters,
        Word[] allWords)
    {
        return allWords
            .Where(word =>
                DictionariesAreEqual(
                    inputLetters,
                    word.WordLetterCount))
            .Select(word => word.Text)
            .ToHashSet();
    }
    private HashSet<string> FindTwoWordAnagrams(
        Dictionary<char, int> inputLetters,
        Word[] allWords)
    {
        var results = new HashSet<string>();

        foreach (Word firstWord in allWords)
        {
            if (!CanUseWord(
                    inputLetters,
                    firstWord.WordLetterCount))
            {
                continue;
            }

            Dictionary<char, int> remainingLetters =
                SubtractLetters(
                    inputLetters,
                    firstWord.WordLetterCount);

            IEnumerable<Word> matchingSecondWords =
                allWords.Where(secondWord =>
                    UsesAllRemainingLetters(
                        secondWord,
                        remainingLetters));

            foreach (Word secondWord in matchingSecondWords)
            {
                results.Add(
                    FormatTwoWords(
                        firstWord,
                        secondWord));
            }
        }

        return results;
    }
    private HashSet<string> FindThreeWordAnagrams(
        Dictionary<char, int> inputLetters,
        Word[] allWords)
    {
        var results = new HashSet<string>();

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

            IEnumerable<Word> possibleSecondWords =
                allWords.Where(secondWord =>
                    CanUseWord(
                        afterFirstWord,
                        secondWord.WordLetterCount));

            foreach (Word secondWord in possibleSecondWords)
            {
                Dictionary<char, int> afterSecondWord =
                    SubtractLetters(
                        afterFirstWord,
                        secondWord.WordLetterCount);

                IEnumerable<Word> possibleThirdWords =
                    allWords.Where(thirdWord =>
                        UsesAllRemainingLetters(
                            thirdWord,
                            afterSecondWord));

                foreach (Word thirdWord in possibleThirdWords)
                {
                    bool canFormat =
                        TryFormatThreeWords(
                            firstWord,
                            secondWord,
                            thirdWord,
                            out string formattedResult);

                    if (canFormat)
                    {
                        results.Add(formattedResult);
                    }
                }
            }
        }
        return results;
    }
    private bool CanUseWord(
        Dictionary<char, int> availableLetters,
        Dictionary<char, int> requiredLetters)
    {
        return requiredLetters.All(requiredLetter =>
            availableLetters.TryGetValue(
                requiredLetter.Key,
                out int availableCount)
            &&
            availableCount >= requiredLetter.Value);
    }
    private Dictionary<char, int> SubtractLetters(
        Dictionary<char, int> availableLetters,
        Dictionary<char, int> usedLetters)
    {
        var remainingLetters =
            new Dictionary<char, int>(availableLetters);

        foreach (KeyValuePair<char, int> usedLetter in usedLetters)
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
    private bool UsesAllRemainingLetters(
        Word word,
        Dictionary<char, int> remainingLetters)
    {
        if (!CanUseWord(
                remainingLetters,
                word.WordLetterCount))
        {
            return false;
        }

        Dictionary<char, int> lettersAfterWord =
            SubtractLetters(
                remainingLetters,
                word.WordLetterCount);

        return lettersAfterWord.Count == 0;
    }
    private bool DictionariesAreEqual(
        Dictionary<char, int> first,
        Dictionary<char, int> second)
    {
        return first.Count == second.Count
            &&
            first.All(pair =>
                second.TryGetValue(
                    pair.Key,
                    out int secondValue)
                &&
                secondValue == pair.Value);
    }
    private string FormatTwoWords(
        Word firstWord,
        Word secondWord)
    {
        Word[] words =
        {
            firstWord,
            secondWord
        };

        Word? adjective =
            words.FirstOrDefault(word =>
                word.Type == SupportedWordTypes.Adjective);

        Word? noun =
            words.FirstOrDefault(word =>
                word.Type == SupportedWordTypes.Noun);

        Word? verb =
            words.FirstOrDefault(word =>
                word.Type == SupportedWordTypes.Verb);

        if (adjective is not null && noun is not null)
        {
            return $"{adjective.Text} {noun.Text}";
        }

        if (noun is not null && verb is not null)
        {
            return $"{noun.Text} {verb.Text}";
        }

        if (adjective is not null && verb is not null)
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
        Word[] words =
        {
            firstWord,
            secondWord,
            thirdWord
        };

        Dictionary<string, List<Word>> wordsByType =
            words
                .GroupBy(word => word.Type)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList());

        bool hasExactlyOneAdjective =
            wordsByType.TryGetValue(
                SupportedWordTypes.Adjective,
                out List<Word>? adjectives)
            &&
            adjectives.Count == 1;

        bool hasExactlyOneNoun =
            wordsByType.TryGetValue(
                SupportedWordTypes.Noun,
                out List<Word>? nouns)
            &&
            nouns.Count == 1;

        bool hasExactlyOneVerb =
            wordsByType.TryGetValue(
                SupportedWordTypes.Verb,
                out List<Word>? verbs)
            &&
            verbs.Count == 1;

        if (!hasExactlyOneAdjective
            || !hasExactlyOneNoun
            || !hasExactlyOneVerb)
        {
            formattedResult = string.Empty;
            return false;
        }
        formattedResult =
            $"{adjectives![0].Text} " +
            $"{nouns![0].Text} " +
            $"{verbs![0].Text}";

        return true;
    }
}