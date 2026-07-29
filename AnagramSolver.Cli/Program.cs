<<<<<<< HEAD
﻿using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using System.Net.Http.Json;
=======
﻿using System.Net.Http.Json;
using AnagramSolver.BusinessLogic;
>>>>>>> origin/feature/AnagramSolver


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
<<<<<<< HEAD
            BaseAddress = new Uri("http://localhost:5053")
        };

        try
        {
            string encodedInput = Uri.EscapeDataString(input);
=======
            var settings = ConfigurationLoader.LoadAnagramSettings();

            var validator = new UserInputValidation(settings);
>>>>>>> origin/feature/AnagramSolver

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

<<<<<<< HEAD
                int countToPrint = Math.Min(
                    results.Count,
                    settings.MaxAnagramsCount);

                logger.Log(
                    $"Maximum anagrams displayed: {countToPrint}");

                results.Take(countToPrint).ToList().ForEach(result => logger.Log(result));
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
=======
                var results = await client.GetFromJsonAsync<List<string>>(
                    $"/api/anagrams/{encodedInput}"
                );
                results ??= new List<string>();

                if (results.Count == 0)
>>>>>>> origin/feature/AnagramSolver
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
<<<<<<< HEAD
                    logger.Log($"Entered string: {input}");

                    return input!
                        .Trim()
                        .ToLower();
=======
                    Console.WriteLine($"Found overall {results.Count} anagrams:");

                    int countToPrint = Math.Min(results.Count, settings.MaxAnagramsCount);

                    Console.WriteLine($"Maximum anagrams displayed: {countToPrint}");

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
>>>>>>> origin/feature/AnagramSolver
                }

                logger.Log("Try again.");
                input = Console.ReadLine();
            }
<<<<<<< HEAD
=======
            catch (HttpRequestException exception)
            {
                Console.WriteLine($"Could not contact the API: {exception.Message}");
            }
            string ReadValidUserInput()
            {
                Console.WriteLine("Enter a phrase: ");

                Console.WriteLine(
                    $"Only letters and spaces allowed. "
                        + $"Must include at least "
                        + $"{settings.MinimumWordLength} characters."
                );

                string? input = Console.ReadLine();

                while (true)
                {
                    if (!validator.ValidateLength(input))
                    {
                        Console.WriteLine(
                            $"Input string must be at least "
                                + $"{settings.MinimumWordLength} characters."
                        );
                    }
                    else if (!validator.ContainsOnlyLettersAndWhitespace(input))
                    {
                        Console.WriteLine("Input string must contain only spaces and letters.");
                    }
                    else
                    {
                        Console.WriteLine($"Entered string: {input}");

                        return input!.Trim().ToLower();
                    }

                    Console.WriteLine("Try again.");
                    input = Console.ReadLine();
                }
            }
>>>>>>> origin/feature/AnagramSolver
        }
    }
}
