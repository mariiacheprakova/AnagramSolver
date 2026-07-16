using AnagramSolver.Contracts.Models;
using System.Text;

namespace AnagramSolver.BusinessLogic;

public class FileWordRepository : IWordRepository
{
    public Word[] GetAllWords(string fileName)
    {
        string[] lines =
            File.ReadAllLines(fileName, Encoding.UTF8);

        var parser =
            new WordFileParser();

        return parser.ParseWords(lines);
    }
}