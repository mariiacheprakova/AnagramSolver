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
        CancellationToken cancellationToken)
    {
        Word[] words =
            await _wordRepository.GetAllWordsAsync(cancellationToken);

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

        return NoContent();
    }
}