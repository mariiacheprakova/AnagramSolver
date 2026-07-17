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
    public Word[] GetAllWords()
    {
        string[] lines =
            File.ReadAllLines(_settings.TextFileName, Encoding.UTF8);

        var parser =
            new WordFileParser();

        return parser.ParseWords(lines);
    }
}