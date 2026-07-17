namespace AnagramSolver.BusinessLogic;

public class LetterCounter
{
    public Dictionary<char, int> CountLetters(string text)
    {
        var letterCount = new Dictionary<char, int>();

        foreach (char character in text.ToLower())
        {
            if (!char.IsLetter(character))
            {
                continue;
            }

            if (letterCount.ContainsKey(character))
            {
                letterCount[character]++;
            }
            else
            {
                letterCount[character] = 1;
            }
        }

        return letterCount;
    }
}