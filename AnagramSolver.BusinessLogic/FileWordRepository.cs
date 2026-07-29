using System.Text;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;

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

    public async Task<Word[]> GetAllWordsAsync(CancellationToken cancellationToken = default)
    {
<<<<<<< HEAD
        string[] lines = await
            File.ReadAllLinesAsync(_settings.TextFileName, Encoding.UTF8, cancellationToken);
=======
        if (_cachedWords is not null)
        {
            return _cachedWords;
        }
>>>>>>> origin/feature/AnagramSolver

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

        bool wordAlreadyExists = existingWords.Any(existingWord => existingWord.Text == word.Text && existingWord.Type == word.Type); 
        if(wordAlreadyExists)
        {
<<<<<<< HEAD
            throw new InvalidOperationException($"The word '{word.Text}' with type '{word.Type}' already exists.");
=======
            if (existingWord.Text == word.Text && existingWord.Type == word.Type)
            {
                throw new InvalidOperationException("Word already exists.");
            }
>>>>>>> origin/feature/AnagramSolver
        }

        string newLine = $"{word.Text} {word.Type}";

        await File.AppendAllTextAsync(
            _settings.TextFileName,
            newLine + Environment.NewLine,
            Encoding.UTF8,
            cancellationToken
        );
        _cachedWords = null;

        word.Id = existingWords.Length + 1;

<<<<<<< HEAD
        word.WordLetterCount =
            LetterCounter.CountLetters(word.Text);
=======
        word.WordLetterCount = WordFileParser.CountLetters(word.Text);
>>>>>>> origin/feature/AnagramSolver

        return word;
    }

<<<<<<< HEAD
    public async Task<bool> DeleteWordByIdAsync(int id, CancellationToken cancellationToken = default)
=======
    public async Task<bool> DeleteWordByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
>>>>>>> origin/feature/AnagramSolver
    {
        Word[] words = await GetAllWordsAsync(cancellationToken);

        bool wordExists = words.Any(word => word.Id == id);
     
        if (!wordExists)
        {
            return false;
        }

        List<string> remainingLines = words.Where(word => word.Id != id).Select(word => $"{word.Text} {word.Type}").ToList();

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
