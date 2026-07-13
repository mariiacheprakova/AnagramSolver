
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AnagramSolver.Cli{ 


class Program
{
    static void Main(string[] args)
    {
        AnagramSettings settings = ConfigurationLoader.LoadAnagramSettings();
        UserInputValidation validator = new UserInputValidation(settings);
        ConsoleConfiguration.ConfigureEncoding();

        string input = UserInput();

        string UserInput()
        {

            Console.WriteLine("Enter a phrase: ");
            Console.WriteLine($"Only characters allowed. Must include at least {settings.MinimumWordLength} characters.");
            string? input = Console.ReadLine()?.Trim().ToLower();

            while (true)
            {

                bool isLengthValid = validator.ValidateLength(input);
                bool isInputVariationValid = validator.ValidateStringType(input);


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

        IWordRepository repository = new FileWordRepository();
        IList<string> lines = repository.GetAllWords("zodynas.txt");
        Console.WriteLine("Words are successfully uploaded from a text file.");
        Console.WriteLine($"Number of lines: {lines.Count}.");

        FormatingUserInput inputFormating = new FormatingUserInput();
        string[] userWords = inputFormating.StringSeparationByWords(input);

        Console.WriteLine($"String contains {userWords.Length} words.");

        for (int i = 0; i < userWords.Length; i++)
        {
            Console.WriteLine($"{i + 1}: {userWords[i]}");
        }
        string userInputString = inputFormating.UserInputString(userWords);
        Console.WriteLine($"Trimmed string without white spaces: {userInputString}");


        Dictionary<char, int> userInputDictionary = inputFormating.LetterCount(userInputString);


        TextFileSeparatingByWordTypes wordsByType = new TextFileSeparatingByWordTypes();
        IList<Word> listOfFileWords = wordsByType.WordObjectCreation(lines);


        AnagramSolverService anagrams = new AnagramSolverService();
       
        IList<string> results = anagrams.GetAnagrams(userInputDictionary,listOfFileWords);
        if (results.Count == 0)
        {
            Console.WriteLine("No anagrams were found.");
        }
        else
        {
            Console.WriteLine($"Found overall {results.Count()} anagrams: ");
            
            int countToPrint = Math.Min(results.Count, settings.MaxAnagramsCount);
            Console.WriteLine($"Maximum anagrams displayed: {countToPrint}");
            for (int i = 0; i < countToPrint; i++)
            {
                Console.WriteLine(results[i]);
            }

        }



    }
}

}
