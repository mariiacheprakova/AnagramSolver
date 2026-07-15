public interface IWordRepository
{
    IList<string> GetAllWords(string fileName);
}