
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AnagramSolver.Cli
{


    class Program
    {
        static void Main(string[] args)
        {
            var settings = ConfigurationLoader.LoadAnagramSettings();
            var validator = new UserInputValidation(settings);
            ConsoleConfiguration.ConfigureEncoding();

            var input = UserInput();

            string UserInput()
            {

                Console.WriteLine("Enter a phrase: ");
                Console.WriteLine($"Only characters allowed. Must include at least {settings.MinimumWordLength} characters.");
                string? input = Console.ReadLine()?.Trim().ToLower();

                while (true)
                {

                    var isLengthValid = validator.ValidateLength(input);
                    var isInputVariationValid = validator.ValidateStringType(input);


                    if (!isLengthValid)
                    {
                        Console.WriteLine($"Input string must be atleast {settings.MinimumWordLength} characters,");

                    }
                    else if (!isInputVariationValid)
                    {
                        Console.WriteLine("Input string must contain only characters.");
                    }
                    else
                    {
                        Console.WriteLine($"Entered string: {input}");
                        return input;
                    }
                    Console.WriteLine("Try again.");
                    input = Console.ReadLine()?.Trim().ToLower();

                }

            }

            var repository = new FileWordRepository(); // creating IList<Word> lines
            var solver = new AnagramSolverService(repository);

            Console.WriteLine("Words are successfully uploaded from a text file.");
            //Console.WriteLine($"Number of lines: {lines.Count}.");

            var inputFormating = new FormatingUserInput();
            var userWords = inputFormating.StringSeparationByWords(input);

            Console.WriteLine($"String contains {userWords.Length} words.");

            for (int i = 0; i < userWords.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {userWords[i]}");
            }
            var userInputString = inputFormating.UserInputString(userWords); // user input string
            Console.WriteLine($"Trimmed string without white spaces: {userInputString}");


            var userInputDictionary = inputFormating.LetterCount(userInputString); // user input dict


            //var wordsByType = new TextFileSeparatingByWordTypes();
            //IList<Word> listOfFileWords = wordsByType.WordObjectCreation(repository);

            //var anagrams = new AnagramSolverService();

            var results = solver.GetAnagrams(userInputDictionary);

            if (results.Count == 0)
            {
                Console.WriteLine("No anagrams were found.");
            }
            else
            {
                Console.WriteLine($"Found overall {results.Count()} anagrams: ");

                var countToPrint = Math.Min(results.Count, settings.MaxAnagramsCount);
                Console.WriteLine($"Maximum anagrams displayed: {countToPrint}");
                for (int i = 0; i < countToPrint; i++)
                {
                    Console.WriteLine(results[i]);
                }

            }



        }
    }

}
