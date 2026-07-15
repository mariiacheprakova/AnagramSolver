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
    public void GetAnagrams_ShouldReturnEmpty_WhenInputIsEmpty()
    {
      
        _repository
            .Setup(r => r.GetAllWords(It.IsAny<string>()))
            .Returns(new List<Word>());

        var solver = new AnagramSolverService(_repository.Object);
        var input = new Dictionary<char, int>();

        var result = solver.GetAnagrams(input);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAnagrams_ShouldReturnEmpty_WhenNoAnagramsExist()
    {

        _repository
            .Setup(r => r.GetAllWords(It.IsAny<string>()))
            .Returns(new List<Word>());
        // Arrange
        var solver = new AnagramSolverService(_repository.Object);
        var input = new Dictionary<char, int>
        {
            ['a'] = 1,
            ['b'] = 1,
            ['c'] = 1
        };

        var words = new List<Word>
        {
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
        };

        var result = solver.GetAnagrams(input);
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAnagrams_ShouldReturnOneWordAnagram_WhenExactMatchExists()
    {
            // Arrange
            
            var solver = new AnagramSolverService(_repository.Object);
            var input = new Dictionary<char, int>
            {
                ['a'] = 1,
                ['b'] = 1,
                ['c'] = 1
            };

            var words = new List<Word>
            {
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
            };
            _repository
               .Setup(r => r.GetAllWords(It.IsAny<string>()))
               .Returns(words);

            var result = solver.GetAnagrams(input);

            Assert.Single(result);
            Assert.Contains("cab",result);
     }
 }
