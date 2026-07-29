<<<<<<< HEAD
namespace AnagramSolver.BusinessLogic;

public interface IAnagramSolver
{
    Task<IReadOnlyCollection<string>> GetAnagramsAsync(Dictionary<char, int> userInputDictionary, CancellationToken cancellationToken = default);
}
=======
namespace AnagramSolver.Contracts;
public interface IAnagramSolver
{
    Task<IReadOnlyCollection<string>> GetAnagramsAsync(
        Dictionary<char, int> userInputDictionary,
        CancellationToken cancellationToken = default
    );
}
>>>>>>> origin/feature/AnagramSolver
