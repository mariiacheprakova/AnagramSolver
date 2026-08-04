using System.Text.RegularExpressions;

namespace AnagramSolver.BusinessLogic
{
    public class FormatingUserInput
    {
        private readonly LetterCounter _letterCounter;
        public FormatingUserInput(LetterCounter letterCounter) => _letterCounter = letterCounter;
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
            return _letterCounter.CountLetters(input);
        }
    }
}
