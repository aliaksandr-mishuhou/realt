using System;
using Microsoft.Extensions.Logging;

namespace Realt.Parser
{
    public class DummyLogger<T> : ILogger<T>
    {
        private const LogLevel MinLevel = LogLevel.Information;

        public IDisposable BeginScope<TState>(TState state)
        {
            return new Scope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (int)logLevel >= (int)MinLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            Console.WriteLine("{0}: {1}", logLevel, formatter(state, exception));
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }

        private class Scope : IDisposable
        {
            public void Dispose()
            {
                // do nothing
            }
        }
    }
}
