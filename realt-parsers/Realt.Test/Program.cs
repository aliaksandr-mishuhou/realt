using System;
using Serilog;
using Serilog.Formatting.Compact;

namespace Realt.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Http("http://localhost:5000", textFormatter: new CompactJsonFormatter())
                .CreateLogger();

            log.Information($"Hello ELK! updated");

            log.Dispose();
        }
    }
}
