using System.Text;

namespace AnagramSolver.Cli
{
    public static class ConsoleConfiguration
    {
        public static void ConfigureUtf8Encoding()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
        }
    }
}
