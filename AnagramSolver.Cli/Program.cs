
using AnagramSolver.BusinessLogic;


namespace AnagramSolver.Cli
{


    class Program
    {
        static void Main(string[] args)
        {
            var settings = ConfigurationLoader.LoadAnagramSettings();
            var validator = new UserInputValidation(settings);
            ConsoleConfiguration.ConfigureUtf8Encoding();

            var input = ReadValidUserInput();

            string ReadValidUserInput()
            {

                Console.WriteLine("Enter a phrase: ");
                Console.WriteLine($"Only letters and spaces allowed. Must include at least {settings.MinimumWordLength} characters.");
                string? input = Console.ReadLine();
                    

                while (true)
                {

                    if (!validator.ValidateLength(input))
                    {
                        Console.WriteLine($"Input string must be atleast {settings.MinimumWordLength} characters,");

                    }
                    else if (!validator.ContainsOnlyLettersAndWhitespace(input))
                    {
                        Console.WriteLine("Input string must contain only spaces and letters.");
                    }
                    else
                    {
                        Console.WriteLine($"Entered string: {input}");
                        return input.Trim().ToLower();
                    }
                    Console.WriteLine("Try again.");
                    input = Console.ReadLine();

                }

            }

            var repository = new FileWordRepository(); 
            var solver = new AnagramSolverService(repository);

            Console.WriteLine("Words are successfully uploaded from a text file.");
         

            var inputFormating = new FormatingUserInput();
            var userWords = inputFormating.StringSeparationByWords(input);

            Console.WriteLine($"String contains {userWords.Length} words.");

            for (int i = 0; i < userWords.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {userWords[i]}");
            }
            var userInputString = inputFormating.UserInputString(userWords);
            Console.WriteLine($"Trimmed string without white spaces: {userInputString}");


            var userInputDictionary = inputFormating.LetterCount(userInputString); 

            var results = solver.GetAnagrams(userInputDictionary);

            if (results.Count == 0)
            {
                Console.WriteLine("No anagrams were found.");
            }
            else
            {
                Console.WriteLine($"Found overall {results.Count} anagrams:");

                int countToPrint = Math.Min(
                    results.Count,
                    settings.MaxAnagramsCount);

                Console.WriteLine(
                    $"Maximum anagrams displayed: {countToPrint}");

                int printedCount = 0;

                foreach (string result in results)
                {
                    if (printedCount >= countToPrint)
                    {
                        break;
                    }

                    Console.WriteLine(result);
                    printedCount++;
                }
            }



        }
    }

}
