using AnagramSolver.Contracts.Models;
using System.Text;

namespace AnagramSolver.BusinessLogic;

public class FileWordRepository : IWordRepository
{
    private readonly AnagramSettings _settings;
    private readonly WordFileParser _parser;

    private Word[]? _cachedWords;
    public FileWordRepository(AnagramSettings settings)
    {
        _settings = settings;
        _parser = new WordFileParser();
    }
    public async Task<Word[]> GetAllWordsAsync(CancellationToken cancellationToken = default
        )
    {
        if(_cachedWords is not null)
        {
            return _cachedWords;
        }

        string[] lines = await
            File.ReadAllLinesAsync(_settings.TextFileName, Encoding.UTF8, cancellationToken);


        _cachedWords = _parser.ParseWords(lines);

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
        foreach (Word existingWord in existingWords)
        {
            if (existingWord.Text == word.Text &&
                existingWord.Type == word.Type)
            {
                throw new InvalidOperationException(
                    "Word already exists.");
            }
        }

        string newLine =
            $"{word.Text} {word.Type}";

        await File.AppendAllTextAsync(
            _settings.TextFileName,
            newLine + Environment.NewLine,
            Encoding.UTF8,
            cancellationToken);
        _cachedWords = null;

        word.Id = existingWords.Length + 1;

        word.WordLetterCount =
            _parser.CountLetters(word.Text);

        return word;
    }

    public async Task<bool> DeleteWordByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Word[] words =
        await GetAllWordsAsync(cancellationToken);

        bool found = false;

        List<string> remainingLines = new();

        foreach (Word word in words)
        {
            if (word.Id == id)
            {
                found = true;
                continue;
            }

            remainingLines.Add($"{word.Text} {word.Type}");
        }

        if (!found)
        {
            return false;
        }

        await File.WriteAllLinesAsync(
            _settings.TextFileName,
            remainingLines,
            Encoding.UTF8,
            cancellationToken);

        _cachedWords = null;

        return true;
    }
}