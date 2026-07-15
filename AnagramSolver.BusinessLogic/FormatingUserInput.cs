using System;
using System.Collections.Generic;
using System.Text;
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

        public Dictionary<char, int> LetterCount(string input)
        {
            return input.GroupBy(c => c).ToDictionary(group => group.Key, group => group.Count());

        }

    }
}
