//using AnagramSolver.BusinessLogic;
//using AnagramSolver.Contracts.Models;
//using FluentAssertions;
//using Xunit;

//namespace AnagramSolver.Tests;

//public class AnagramSolverServiceTests
//{
//    [Fact]
//    public void GetAnagrams_ShouldReturnEmpty_WhenInputIsEmpty()
//    {
//        // Arrange
//        AnagramSolverService solver = new();

//        Dictionary<char, int> input = new();

//        IList<Word> words = new List<Word>();

//        // Act
//        IList<string> result = solver.GetAnagrams(input, words);

//        // Assert
//        result.Should().BeEmpty();
//    }

//    [Fact]
//    public void GetAnagrams_ShouldReturnEmpty_WhenNoAnagramsExist()
//    {
//        // Arrange
//        AnagramSolverService solver = new();

//        Dictionary<char, int> input = new()
//        {
//            ['a'] = 1,
//            ['b'] = 1,
//            ['c'] = 1
//        };

//        IList<Word> words = new List<Word>
//        {
//            new Word
//            {
//                Text = "dog",
//                Type = "bdv",
//                WordLetterCount = new Dictionary<char,int>
//                {
//                    ['d']=1,
//                    ['o']=1,
//                    ['g']=1
//                }
//            }
//        };

//        // Act
//        IList<string> result = solver.GetAnagrams(input, words);

//        // Assert
//        result.Should().BeEmpty();
//    }

//    [Fact]
//    public void GetAnagrams_ShouldReturnOneWordAnagram_WhenExactMatchExists()
//    {
//        // Arrange
//        AnagramSolverService solver = new();

//        Dictionary<char, int> input = new()
//        {
//            ['a'] = 1,
//            ['b'] = 1,
//            ['c'] = 1
//        };

//        IList<Word> words = new List<Word>
//        {
//            new Word
//            {
//                Text = "cab",
//                Type = "bdv",
//                WordLetterCount = new Dictionary<char,int>
//                {
//                    ['a']=1,
//                    ['b']=1,
//                    ['c']=1
//                }
//            }
//        };

//        // Act
//        IList<string> result = solver.GetAnagrams(input, words);

//        // Assert
//        result.Should().ContainSingle();
//        result.Should().Contain("cab");
//    }
//}