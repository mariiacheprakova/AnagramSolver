using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AnagramSolver.BusinessLogic;

public class UserInputValidation
{
    private readonly AnagramSettings _settings;
    public UserInputValidation(AnagramSettings settings)
    {
        _settings = settings;
    }

    public bool ValidateLength(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }
        return input.Length >= _settings.MinimumWordLength;
    }

    public bool ValidateStringType(string input)
    {
        return Regex.IsMatch(input, @"^[\p{L}\s]+$");
    }


}
