using System.Text;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic;
public class FileWordRepository : IWordRepository
{
    private readonly AnagramSettings _settings;
    private Word[]? _cachedWords;
    public FileWordRepository(AnagramSettings settings)
    {
        _settings = settings;
    }
    public async Task<Word[]> GetAllWordsAsync(CancellationToken cancellationToken = default)
    {
        if (_cachedWords is not null)
        {
            return _cachedWords;
        }

        string[] lines = await File.ReadAllLinesAsync(
            _settings.TextFileName,
            Encoding.UTF8,
            cancellationToken
        );

        _cachedWords = WordFileParser.ParseWords(lines);
        return _cachedWords;
    }
    public async Task<Word?> GetWordByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Word[] words = await GetAllWordsAsync(cancellationToken);
        return words.FirstOrDefault(word => word.Id == id);
    }
    public async Task<Word?> AddWordAsync(Word word, CancellationToken cancellationToken = default)
    {
        Word[] existingWords = await GetAllWordsAsync(cancellationToken);

        var wordAlreadyExists = existingWords.Any(existingWord => existingWord.Text == word.Text && existingWord.Type == word.Type);
        if (wordAlreadyExists)
        {
            throw new InvalidOperationException("Word already exists.");
        }

        string newLine = $"{word.Text} {word.Type}";
        await File.AppendAllTextAsync(
            _settings.TextFileName,
            newLine + Environment.NewLine,
            Encoding.UTF8,
            cancellationToken
        );
        _cachedWords = null;

        var maxId = existingWords.Length > 0 ? existingWords.Max(w => w.Id) : 0;
        word.Id = maxId + 1;
        word.WordLetterCount = LetterCounter.CountLetters(word.Text);
        return word;
    }
    public async Task<bool> DeleteWordByIdAsync(int id,CancellationToken cancellationToken = default)
    {
        Word[] words = await GetAllWordsAsync(cancellationToken);
        bool wordExists = words.Any(word => word.Id == id);

        if (!wordExists)
        {
            return false;
        }

        List<string> remainingLines = words
            .Where(word => word.Id != id)
            .Select(word => $"{word.Text} {word.Type}")
            .ToList();
        await File.WriteAllLinesAsync(
            _settings.TextFileName,
            remainingLines,
            Encoding.UTF8,
            cancellationToken
        );
        _cachedWords = null;
        return true;
    }
}

