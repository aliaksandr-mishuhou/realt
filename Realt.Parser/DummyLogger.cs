using System;
using Microsoft.Extensions.Logging;

namespace Realt.Parser
{
    public class DummyLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return new Scope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine("{0}: {1} {2}", logLevel, formatter(state, exception), exception);
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
