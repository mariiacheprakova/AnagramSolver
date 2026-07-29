using Microsoft.AspNetCore.Mvc;
using AnagramSolver.BusinessLogic;
using System.Diagnostics;

namespace AnagramSolver.WebApp.Controllers;


[ApiController]
[Route("api/[controller]")]

public class AnagramsController : ControllerBase
{
    private readonly IAnagramSolver _anagramSolver;
  

    public AnagramsController(IAnagramSolver anagramSolver)
    {
        _anagramSolver = anagramSolver;
       
    }

    [HttpGet("{word}")]
    public async Task<ActionResult<IReadOnlyCollection<string>>> GetAnagramsAsync(string word, CancellationToken cancellationToken)

    {
        var stopwatch = Stopwatch.StartNew();

        var wordDictionary = LetterCounter.CountLetters(word);
        var anagrams = await _anagramSolver.GetAnagramsAsync(wordDictionary, cancellationToken);
        stopwatch.Stop();

        Response.Headers.Append("X-Anagram-Count", anagrams.Count().ToString());
        Response.Headers.Append("X-Seacrh-Duration-Ms", stopwatch.ElapsedMilliseconds.ToString());
        return Ok(anagrams);
    }
}



