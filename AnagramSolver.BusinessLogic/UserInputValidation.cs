using AnagramSolver.Contracts.Models;
using System.Text.RegularExpressions;

namespace AnagramSolver.BusinessLogic;

public class UserInputValidation
{
    private readonly AnagramSettings _settings;
    public UserInputValidation(AnagramSettings settings)
    {
        _settings = settings;
    }

    public bool ValidateLength(string? input)
    {
        return !string.IsNullOrWhiteSpace(input)
            && input.Length >= _settings.MinimumWordLength;
    }

    public bool ContainsOnlyLettersAndWhitespace(string? input)
    {
        return !string.IsNullOrWhiteSpace(input)
             && Regex.IsMatch(input, @"^[\p{L}\s]+$");
    }


}
