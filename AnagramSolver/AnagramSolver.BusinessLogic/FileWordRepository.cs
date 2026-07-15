using AnagramSolver.Contracts.Models;
using System.Text;


namespace AnagramSolver.BusinessLogic;

    public class FileWordRepository : IWordRepository
    {
        public IList<Word> GetAllWords(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);
            return (IList<Word>)lines.ToList();

        }
}
