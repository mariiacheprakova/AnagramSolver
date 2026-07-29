using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers;

[ApiController]
[Route("api/words")]
public class WordsApiController : ControllerBase
{
    private readonly IWordRepository _wordRepository;

    public WordsApiController(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<Word>>> GetWordsAsync(
<<<<<<< HEAD
           CancellationToken cancellationToken)
    {
        // http://localhost:5053/api/words?page=2&size=100
        var headers = Request.Headers;
        var method = Request.Method; // GET
        var path = Request.Path; // api.words
        var query = Request.Query; //?page=2&size=100
        var ip = HttpContext.Connection.RemoteIpAddress; //Who sent this request? if local might be ::1
        var cookies = Request.Cookies;


        Word[] words =
            await _wordRepository.GetAllWordsAsync(cancellationToken);

=======
        CancellationToken cancellationToken
    )
    {
        var headers = Request.Headers;
        var method = Request.Method;
        var path = Request.Path;
        var query = Request.Query;
        var ip = HttpContext.Connection.RemoteIpAddress;
        var cookies = Request.Cookies;

        Word[] words = await _wordRepository.GetAllWordsAsync(cancellationToken);

>>>>>>> origin/feature/AnagramSolver
        return Ok(words);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Word>> GetWordByIdAsync(
<<<<<<< HEAD
            int id,
            CancellationToken cancellationToken)
    {
        Word? word =
            await _wordRepository.GetWordByIdAsync(
                id,
                cancellationToken);
=======
        int id,
        CancellationToken cancellationToken
    )
    {
        Word? word = await _wordRepository.GetWordByIdAsync(id, cancellationToken);
>>>>>>> origin/feature/AnagramSolver

        if (word is null)
        {
            return NotFound();
        }

        return Ok(word);
    }

    [HttpPost]
    public async Task<ActionResult<Word>> AddWordAsync(
        Word word,
<<<<<<< HEAD
        CancellationToken cancellationToken)
    {
        Word addedWord =
            await _wordRepository.AddWordAsync(
                word,
                cancellationToken);

        return CreatedAtAction(
            nameof(GetWordByIdAsync),
            new { id = addedWord.Id },
            addedWord);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWordAsync(
        int id,
        CancellationToken cancellationToken)
    {
        bool deleted =
            await _wordRepository.DeleteWordByIdAsync(
                id,
                cancellationToken);
=======
        CancellationToken cancellationToken
    )
    {
        Word addedWord = await _wordRepository.AddWordAsync(word, cancellationToken);

        return CreatedAtAction(nameof(GetWordByIdAsync), new { id = addedWord.Id }, addedWord);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWordAsync(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _wordRepository.DeleteWordByIdAsync(id, cancellationToken);
>>>>>>> origin/feature/AnagramSolver

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/feature/AnagramSolver
