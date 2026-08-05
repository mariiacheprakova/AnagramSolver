using System.Text.RegularExpressions;

namespace AnagramSolver.BusinessLogic
{
    public class FormatingUserInput
    {
        public string[] StringSeparationByWords(string input)
        {
            return Regex.Split(input, @"[\s,]+");
        }
        public string UserInputString(string[] words)
        {
            return string.Concat(words);
        }
        public Dictionary<char, int> CountLetters(string input)
        {
            return LetterCounter.CountLetters(input);
        }
    }
}
