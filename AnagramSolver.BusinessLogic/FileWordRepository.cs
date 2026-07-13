using System.Text;

namespace AnagramSolver.BusinessLogic;

    public class FileWordRepository : IWordRepository
    {
        public IList<string> GetAllWords(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);
            return lines.ToList();

        }
    }
