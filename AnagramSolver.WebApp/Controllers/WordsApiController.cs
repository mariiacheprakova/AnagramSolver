using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers;

[ApiController]
[Route("api/words")]
public class WordsApiController : ControllerBase
{
    private readonly IWordRepository _wordRepository;
    private readonly MemoryCache<IReadOnlyCollection<string>> _cache;

    public WordsApiController(IWordRepository wordRepository, MemoryCache<IReadOnlyCollection<string>> cache)
    {
        _wordRepository = wordRepository;
        _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<Word>>> GetWordsAsync(
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

        return Ok(words);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Word>> GetWordByIdAsync(
            int id,
            CancellationToken cancellationToken)
    {
        Word? word =
            await _wordRepository.GetWordByIdAsync(
                id,
                cancellationToken);

        if (word is null)
        {
            return NotFound();
        }

        return Ok(word);
    }

    [HttpPost]
    public async Task<ActionResult<Word>> AddWordAsync(
        Word word,
        CancellationToken cancellationToken)
    {
        Word addedWord =
            await _wordRepository.AddWordAsync(
                word,
                cancellationToken);

        _cache.Clear();

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

        if (!deleted)
        {
            return NotFound();
        }
        _cache.Clear();
        return NoContent();
    }
}
