using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Cli
{
    public static class ConsoleConfiguration
    {
        public static void ConfigureEncoding()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
        }
    }
}
