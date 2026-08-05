using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic;
public class Logging : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}
