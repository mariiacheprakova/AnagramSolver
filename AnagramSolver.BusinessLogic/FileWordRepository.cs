using AnagramSolver.Contracts.Models;
using System.Text;

namespace AnagramSolver.BusinessLogic;

public class FileWordRepository : IWordRepository
{
    private readonly AnagramSettings _settings;
    public FileWordRepository(AnagramSettings settings)
    {
        _settings = settings;
    }
    public async Task<Word[]> GetAllWordsAsync(CancellationToken cancellationToken = default
        )
    {
        string[] lines = await
            File.ReadAllLinesAsync(_settings.TextFileName, Encoding.UTF8, cancellationToken);

        var parser =
            new WordFileParser();

        return parser.ParseWords(lines);
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
            throw new InvalidOperationException($"The word '{word.Text}' with type '{word.Type}' already exists.");
        }

        string newLine =
            $"{word.Text} {word.Type}";

        await File.AppendAllTextAsync(
            _settings.TextFileName,
            newLine + Environment.NewLine,
            Encoding.UTF8,
            cancellationToken);

        word.Id = existingWords.Length + 1;

        word.WordLetterCount =
            LetterCounter.CountLetters(word.Text);

        return word;
    }

    public async Task<bool> DeleteWordByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Word[] words =
        await GetAllWordsAsync(cancellationToken);

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
            cancellationToken);

        return true;
    }
}