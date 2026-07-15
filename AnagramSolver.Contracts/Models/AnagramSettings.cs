using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Models
{
    public class AnagramSettings
    {
        public int MaxAnagramsCount { get; set; }
        public int MinimumWordLength { get; set; }

    }
    public class Word
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public Dictionary<char, int> WordLetterCount { get; set; }
    }
}
