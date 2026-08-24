using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Repositories;

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
    public async Task<IReadOnlyCollection<string>> GetAnagramsAsync(Dictionary<char, int> userInputDictionary,CancellationToken cancellationToken = default)
    {
        Word[] loadedWords = await _wordRepository.GetAllWordsAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        Word[] supportedWords =GetSupportedWords(loadedWords,userInputDictionary);
        HashSet<string> results =
            FindOneWordAnagrams(
                    userInputDictionary,
                    supportedWords)
                .Union(
                    FindTwoWordAnagrams(
                        userInputDictionary,
                        supportedWords,
                        cancellationToken))
                .Union(
                    FindThreeWordAnagrams(
                        userInputDictionary,
                        supportedWords,
                        cancellationToken))
                .ToHashSet();

        await _searchLogRepository.AddAsync(results.Count, cancellationToken);
        return results;
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
                (word.Text, word.Type))
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
            {
                cancellationToken.ThrowIfCancellationRequested();
                return CanUseWord(inputLetters, firstWord.WordLetterCount);
            })
            .SelectMany(firstWord =>
            {
                Dictionary<char, int> remainingLetters =
                    SubtractLetters(inputLetters, firstWord.WordLetterCount);

                return allWords
                    .Where(secondWord =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return UsesAllRemainingLetters(secondWord, remainingLetters);
                    })
                    .Select(secondWord => FormatTwoWords(firstWord, secondWord));
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
            {
                cancellationToken.ThrowIfCancellationRequested();
                return CanUseWord(inputLetters, firstWord.WordLetterCount);
            })
            .SelectMany(firstWord =>
            {
                Dictionary<char, int> afterFirstWord =
                    SubtractLetters(inputLetters, firstWord.WordLetterCount);

                return allWords
                    .Where(secondWord =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return CanUseWord(afterFirstWord, secondWord.WordLetterCount);
                    })
                    .SelectMany(secondWord =>
                    {
                        Dictionary<char, int> afterSecondWord =SubtractLetters(afterFirstWord, secondWord.WordLetterCount);
                        return allWords
                            .Where(thirdWord =>
                            {
                                cancellationToken.ThrowIfCancellationRequested();

                                return UsesAllRemainingLetters(thirdWord, afterSecondWord);
                            })
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
                                    FormattedResult = formattedResult
                                };
                            });
                    });
            })
            .Where(result => result.CanFormat)
            .Select(result => result.FormattedResult)
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
    private Dictionary<char, int> SubtractLetters(Dictionary<char, int> availableLetters, Dictionary<char, int> usedLetters)
    {
        var remainingLetters =
            new Dictionary<char, int>(availableLetters);
        usedLetters
            .ToList()
            .ForEach(usedLetter =>
            {
                remainingLetters[usedLetter.Key] -=
                    usedLetter.Value;

                if (remainingLetters[usedLetter.Key] == 0)
                {
                    remainingLetters.Remove(
                        usedLetter.Key);
                }
            });
        return remainingLetters;
    }
    private bool UsesAllRemainingLetters(Word word, Dictionary<char, int> remainingLetters)
    {
        return CanUseWord(
                   remainingLetters,
                   word.WordLetterCount)
               &&
               SubtractLetters(
                       remainingLetters,
                       word.WordLetterCount)
                   .Count == 0;
    }
    private bool DictionariesAreEqual(Dictionary<char, int> first, Dictionary<char, int> second)
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
    private string FormatTwoWords(Word firstWord,Word secondWord)
    {
        Word[] words =
        {
            firstWord,
            secondWord
        };

        Word? adjective =words.FirstOrDefault(word =>word.Type == SupportedWordTypes.Adjective);
        Word? noun = words.FirstOrDefault(word =>word.Type == SupportedWordTypes.Noun);
        Word? verb =words.FirstOrDefault(word =>word.Type == SupportedWordTypes.Verb);

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

        ILookup<string?, Word> wordsByType =
            words.ToLookup(word => word.Type);

        if (wordsByType[SupportedWordTypes.Adjective].Count() != 1 ||
            wordsByType[SupportedWordTypes.Noun].Count() != 1 ||
            wordsByType[SupportedWordTypes.Verb].Count() != 1)
        {
            formattedResult = string.Empty;
            return false;
        }

        Word adjective = wordsByType[SupportedWordTypes.Adjective].Single();
        Word noun = wordsByType[SupportedWordTypes.Noun].Single();
        Word verb = wordsByType[SupportedWordTypes.Verb].Single();
        formattedResult =
            $"{adjective.Text} {noun.Text} {verb.Text}";
        return true;
    }
}