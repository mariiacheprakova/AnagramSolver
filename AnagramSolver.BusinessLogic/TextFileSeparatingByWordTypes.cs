using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.BusinessLogic
{
    public class WordFileParser
    {

        public List<Word> WordObjectCreation(IList<string> lines)
        {
            List<Word> fileWords = new List<Word>();

            foreach (string line in lines)
            {
                Word word = new Word();
                string[] parts = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries); // splits on any ws
                word.Text = parts[0];
                word.Type = parts[1];
                word.WordLetterCount = parts[0].GroupBy(c => c).ToDictionary(group => group.Key, group => group.Count());
                fileWords.Add(word);

            }

            return fileWords;

        }
    }
}
