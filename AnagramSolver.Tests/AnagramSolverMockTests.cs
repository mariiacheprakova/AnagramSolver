using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using FluentAssertions;
using Moq;

namespace AnagramSolver.Tests;

public class AnagramSolverServiceMockTests
{
    private readonly Mock<IWordRepository> _repository = new();
    private readonly Mock<IWordFilter> _filterChain = new();

    public AnagramSolverServiceMockTests()
    {
        _filterChain
            .Setup(filter => filter.Handle(
                It.IsAny<Word>(),
                It.IsAny<Dictionary<char, int>>()))
            .Returns(true);
    }

    [Fact]
    public async Task GetAnagramsAsync_ShouldReturnEmpty_WhenInputIsEmpty()
    {
        // Arrange
        _repository
            .Setup(repository =>
                repository.GetAllWordsAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Word>());

        var solver =
            new AnagramSolverService(
                _repository.Object,
                _filterChain.Object);

        var input =
            new Dictionary<char, int>();

        // Act
        IReadOnlyCollection<string> result =
            await solver.GetAnagramsAsync(input);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAnagramsAsync_ShouldReturnEmpty_WhenNoAnagramsExist()
    {
        // Arrange
        var input = new Dictionary<char, int>
        {
            ['a'] = 1,
            ['b'] = 1,
            ['c'] = 1,
        };

        Word[] words =
        [
            new Word
            {
                Text = "dog",
                Type = SupportedWordTypes.Adjective,
                WordLetterCount = new Dictionary<char, int>
                {
                    ['d'] = 1,
                    ['o'] = 1,
                    ['g'] = 1,
                },
            },
        ];

        _repository
            .Setup(repository =>
                repository.GetAllWordsAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(words);

        var solver =
            new AnagramSolverService(
                _repository.Object,
                _filterChain.Object);

        // Act
        IReadOnlyCollection<string> result =
            await solver.GetAnagramsAsync(input);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAnagramsAsync_ShouldReturnOneWordAnagram_WhenExactMatchExists()
    {
        // Arrange
        var input = new Dictionary<char, int>
        {
            ['a'] = 1,
            ['b'] = 1,
            ['c'] = 1,
        };

        Word[] words =
        [
            new Word
            {
                Text = "cab",
                Type = SupportedWordTypes.Adjective,
                WordLetterCount = new Dictionary<char, int>
                {
                    ['a'] = 1,
                    ['b'] = 1,
                    ['c'] = 1,
                },
            },
        ];
        _repository
            .Setup(r => r.GetAllWordsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(words);

        var solver =
            new AnagramSolverService(
                _repository.Object,
                _filterChain.Object);

        // Act
        var result = await solver.GetAnagramsAsync(input);

        // Assert
        Assert.Single(result);
        Assert.Contains("cab", result);
    }
}