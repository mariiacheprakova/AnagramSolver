using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using System.Net.Http.Json;

namespace AnagramSolver.Cli;

class Program
{
    static async Task Main(string[] args)
    {
        ILogger logger = new Logging();

        var settings =
            ConfigurationLoader.LoadAnagramSettings();

        var validator =
            new UserInputValidation(settings);

        ConsoleConfiguration.ConfigureUtf8Encoding();

        string input = ReadValidUserInput();

        using var client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5053")
        };

        try
        {
            string encodedInput =
                Uri.EscapeDataString(input);

            var results =
                await client.GetFromJsonAsync<List<string>>(
                    $"/api/anagrams/{encodedInput}");

            results ??= new List<string>();

            if (results.Count == 0)
            {
                logger.Log("No anagrams were found.");
            }
            else
            {
                logger.Log(
                    $"Found overall {results.Count} anagrams:");

                int countToPrint =
                    Math.Min(
                        results.Count,
                        settings.MaxAnagramsCount);

                logger.Log(
                    $"Maximum anagrams displayed: {countToPrint}");

                foreach (string result in results.Take(countToPrint))
                {
                    logger.Log(result);
                }
            }
        }
        catch (HttpRequestException exception)
        {
            logger.Log(
                $"Could not contact the API: {exception.Message}");
        }

        string ReadValidUserInput()
        {
            logger.Log("Enter a phrase:");

            logger.Log(
                $"Only letters and spaces allowed. " +
                $"Must include at least " +
                $"{settings.MinimumWordLength} characters.");

            string? input = Console.ReadLine();

            while (true)
            {
                if (!validator.ValidateLength(input))
                {
                    logger.Log(
                        $"Input string must be at least " +
                        $"{settings.MinimumWordLength} characters.");
                }
                else if (!validator.ContainsOnlyLettersAndWhitespace(input))
                {
                    logger.Log(
                        "Input string must contain only spaces and letters.");
                }
                else
                {
                    logger.Log($"Entered string: {input}");

                    return input!
                        .Trim()
                        .ToLower();
                }

                logger.Log("Try again.");
                input = Console.ReadLine();
            }
        }
    }
}