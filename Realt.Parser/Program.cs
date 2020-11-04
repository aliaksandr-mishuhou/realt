using System;

namespace Realt.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            IRunner runner = new Runner(new Parser(), new Repository());
            runner.RunAsync().Wait();
        }
    }
}
