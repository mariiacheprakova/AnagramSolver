using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;

public class AnagramSolverService : IAnagramSolver
{
    private readonly IWordRepository _wordRepository;
    private readonly IWordFilter _filterChain;

    public AnagramSolverService(
        IWordRepository wordRepository,
        IWordFilter filterChain)
    {
        _wordRepository = wordRepository;
        _filterChain = filterChain;
    }

    public async Task<IReadOnlyCollection<string>> GetAnagramsAsync(
        Dictionary<char, int> userInputDictionary,CancellationToken cancellationToken=default)
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
        Word[] allWords,
        CancellationToken cancellationToken)
    {
        return allWords
            .Where(firstWord =>
                CanUseWord(
                    inputLetters,
                    firstWord.WordLetterCount))
            .SelectMany(firstWord =>
            {
                Dictionary<char, int> afterFirstWord =
                    SubtractLetters(
                        inputLetters,
                        firstWord.WordLetterCount);

                return allWords
                    .Where(secondWord =>
                        CanUseWord(
                            afterFirstWord,
                            secondWord.WordLetterCount))
                    .Where(secondWord =>
                        SubtractLetters(
                            afterFirstWord,
                            secondWord.WordLetterCount)
                        .Count == 0)
                    .Select(secondWord =>
                        FormatTwoWords(
                            firstWord,
                            secondWord));
            })
            .ToHashSet();
    }

    private HashSet<string> FindThreeWordAnagrams(
        Dictionary<char, int> inputLetters,
        Word[] allWords,
        CancellationToken cancellationToken)
    {
        return allWords
            .Where(firstWord =>
                CanUseWord(
                    inputLetters,
                    firstWord.WordLetterCount))
            .SelectMany(firstWord =>
            {
                Dictionary<char, int> afterFirstWord =
                    SubtractLetters(
                        inputLetters,
                        firstWord.WordLetterCount);

                return allWords
                    .Where(secondWord =>
                        CanUseWord(
                            afterFirstWord,
                            secondWord.WordLetterCount))
                    .SelectMany(secondWord =>
                    {
                        Dictionary<char, int> afterSecondWord =
                            SubtractLetters(
                                afterFirstWord,
                                secondWord.WordLetterCount);

                        return allWords
                            .Where(thirdWord =>
                                CanUseWord(
                                    afterSecondWord,
                                    thirdWord.WordLetterCount))
                            .Where(thirdWord =>
                                SubtractLetters(
                                    afterSecondWord,
                                    thirdWord.WordLetterCount)
                                .Count == 0)
                            .Select(thirdWord =>
                            {
                                bool canFormat =
                                    TryFormatThreeWords(
                                        firstWord,
                                        secondWord,
                                        thirdWord,
                                        out string formattedResult);

                                return new
                                {
                                    CanFormat = canFormat,
                                    Result = formattedResult
                                };
                            });
                    });
            })
            .Where(result => result.CanFormat)
            .Select(result => result.Result)
            .ToHashSet();
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

        foreach (Word word in words)
        {
            if (word.Type == SupportedWordTypes.Adjective)
            {
                adjective = word;
            }
            else if (word.Type == SupportedWordTypes.Noun)
            {
                noun = word;
            }
            else if (word.Type == SupportedWordTypes.Verb)
            {
                verb = word;
            }
        }

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
                "bdv",
                out List<Word>? adjectives)
            &&
            adjectives.Count == 1;

        bool hasExactlyOneNoun =
            wordsByType.TryGetValue(
                "dkt",
                out List<Word>? nouns)
            &&
            nouns.Count == 1;

        bool hasExactlyOneVerb =
            wordsByType.TryGetValue(
                "vksm",
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