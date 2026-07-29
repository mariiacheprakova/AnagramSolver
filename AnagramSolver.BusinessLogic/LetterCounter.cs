namespace AnagramSolver.BusinessLogic;

public class LetterCounter
{
    public static Dictionary<char, int> CountLetters(string text)
    {
        text = text.ToLower();
        var letterCount = text.GroupBy(letter => letter).ToDictionary(group => group.Key, group => group.Count());

        return letterCount;
    }
}
