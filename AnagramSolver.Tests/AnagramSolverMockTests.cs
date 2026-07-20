using Xunit;
using FluentAssertions;
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Models;
using Moq;

namespace AnagramSolver.Tests;

public class AnagramSolverServiceMockTests
{

    private readonly Mock<IWordRepository> _repository = new();

    [Fact]
    //Method_ShouldExpectedBehaviour_WhenCondition - name convention
    public async Task GetAnagramsAsync_ShouldReturnEmpty_WhenInputIsEmpty()
    {

        _repository
            .Setup(r => r.GetAllWordsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Word>());

        var solver = new AnagramSolverService(_repository.Object);
        var input = new Dictionary<char, int>();

        var result = await solver.GetAnagramsAsync(input);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAnagramsAsync_ShouldReturnEmpty_WhenNoAnagramsExist()
    {

         var input = new Dictionary<char, int>
        {
            ['a'] = 1,
            ['b'] = 1,
            ['c'] = 1
        };

        Word[] words =
        [
            new Word
            {
                Text = "dog",
                Type = "bdv",
                WordLetterCount = new Dictionary<char,int>
                {
                    ['d']=1,
                    ['o']=1,
                    ['g']=1
                }
            }
        ];
        
        _repository
            .Setup(r => r.GetAllWordsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(words);

        var solver = new AnagramSolverService(_repository.Object);
      
        var result = await solver.GetAnagramsAsync(input);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAnagramsAsync_ShouldReturnOneWordAnagram_WhenExactMatchExists()
    {

      
        var input = new Dictionary<char, int>
        {
            ['a'] = 1,
            ['b'] = 1,
            ['c'] = 1
        };


        Word[] words =
            [
                new Word
                {
                    Text = "cab",
                    Type = "bdv",
                    WordLetterCount = new Dictionary<char,int>
                    {
                        ['a']=1,
                        ['b']=1,
                        ['c']=1
                    }
                }
            ];
        _repository
           .Setup(r => r.GetAllWordsAsync(It.IsAny<CancellationToken>()))
           .ReturnsAsync(words); 

        var solver = new AnagramSolverService(_repository.Object);

        var result = await solver.GetAnagramsAsync(input);

        Assert.Single(result);
        Assert.Contains("cab", result);
    }
}
