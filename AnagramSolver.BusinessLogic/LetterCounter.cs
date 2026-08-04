namespace AnagramSolver.BusinessLogic;
public static class LetterCounter
{
    public static Dictionary<char, int> CountLetters(string text)
    {
        var letterCount = text.ToLower()
            .Where(char.IsLetter)
            .GroupBy(letter => letter)
            .ToDictionary(group => group.Key, group => group.Count());
        return letterCount;
    }
}
