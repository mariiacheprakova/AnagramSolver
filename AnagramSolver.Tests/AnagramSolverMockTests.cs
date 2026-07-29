<<<<<<< HEAD
﻿using Xunit;
using FluentAssertions;
using AnagramSolver.BusinessLogic;
=======
﻿using AnagramSolver.BusinessLogic;
>>>>>>> origin/feature/AnagramSolver
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

<<<<<<< HEAD
        var input =
            new Dictionary<char, int>();

        // Act
        IReadOnlyCollection<string> result =
            await solver.GetAnagramsAsync(input);

        // Assert
        result.Should().BeEmpty();
=======
        // Act
        var result = await solver.GetAnagramsAsync(input);

        //Assert
        Assert.Empty(result);
>>>>>>> origin/feature/AnagramSolver
    }

    [Fact]
    public async Task GetAnagramsAsync_ShouldReturnEmpty_WhenNoAnagramsExist()
    {
        // Arrange
<<<<<<< HEAD
        var input =
            new Dictionary<char, int>
            {
                ['a'] = 1,
                ['b'] = 1,
                ['c'] = 1
            };
=======
        var input = new Dictionary<char, int>
        {
            ['a'] = 1,
            ['b'] = 1,
            ['c'] = 1,
        };
>>>>>>> origin/feature/AnagramSolver

        Word[] words =
        [
            new Word
            {
                Text = "dog",
<<<<<<< HEAD
                Type = "dkt",
                WordLetterCount =
                    new Dictionary<char, int>
                    {
                        ['d'] = 1,
                        ['o'] = 1,
                        ['g'] = 1
                    }
            }
=======
                Type = SupportedWordTypes.Adjective,
                WordLetterCount = new Dictionary<char, int>
                {
                    ['d'] = 1,
                    ['o'] = 1,
                    ['g'] = 1,
                },
            },
>>>>>>> origin/feature/AnagramSolver
        ];

        _repository
            .Setup(repository =>
                repository.GetAllWordsAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(words);

<<<<<<< HEAD
        var solver =
            new AnagramSolverService(
                _repository.Object,
                _filterChain.Object);

        // Act
        IReadOnlyCollection<string> result =
            await solver.GetAnagramsAsync(input);
=======
        var solver = new AnagramSolverService(_repository.Object);

        // Act
        var result = await solver.GetAnagramsAsync(input);
>>>>>>> origin/feature/AnagramSolver

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAnagramsAsync_ShouldReturnOneWordAnagram_WhenExactMatchExists()
    {
        // Arrange
<<<<<<< HEAD
        var input =
            new Dictionary<char, int>
            {
                ['a'] = 1,
                ['b'] = 1,
                ['c'] = 1
            };
=======
        var input = new Dictionary<char, int>
        {
            ['a'] = 1,
            ['b'] = 1,
            ['c'] = 1,
        };
>>>>>>> origin/feature/AnagramSolver

        Word[] words =
        [
            new Word
            {
                Text = "cab",
<<<<<<< HEAD
                Type = "dkt",
                WordLetterCount =
                    new Dictionary<char, int>
                    {
                        ['a'] = 1,
                        ['b'] = 1,
                        ['c'] = 1
                    }
            }
        ];

        _repository
            .Setup(repository =>
                repository.GetAllWordsAsync(
                    It.IsAny<CancellationToken>()))
=======
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
>>>>>>> origin/feature/AnagramSolver
            .ReturnsAsync(words);

        var solver =
            new AnagramSolverService(
                _repository.Object,
                _filterChain.Object);

        // Act
<<<<<<< HEAD
        IReadOnlyCollection<string> result =
            await solver.GetAnagramsAsync(input);

        // Assert
        result.Should().ContainSingle();
        result.Should().Contain("cab");
=======
        var result = await solver.GetAnagramsAsync(input);

        // Assert
        Assert.Single(result);
        Assert.Contains("cab", result);
>>>>>>> origin/feature/AnagramSolver
    }
}