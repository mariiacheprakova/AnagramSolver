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
            File.ReadAllLinesAsync(_settings.TextFileName, Encoding.UTF8,cancellationToken);

        var parser =
            new WordFileParser();

        return parser.ParseWords(lines);
    }
}