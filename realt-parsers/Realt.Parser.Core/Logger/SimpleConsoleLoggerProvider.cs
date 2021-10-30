using System;
using Microsoft.Extensions.Logging;

namespace Realt.Parser.Core.Logger
{
    [ProviderAlias("RoundTheCodeFile")]
    public class SimpleConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new SimpleConsolerLogger();
        }

        public void Dispose()
        {
        }
    }
}
