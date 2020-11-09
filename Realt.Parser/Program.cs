//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

namespace Realt.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            IRunner runner = new Runner(new Parser(new DummyLogger<Parser>()),
                new AggregatedRepository(new IRepository[]
                {
                    new JsonPageFileRepository(),
                    new CsvRepository()
                }, new DummyLogger<AggregatedRepository>()),
                new DummyLogger<Runner>());
            runner.RunAsync().Wait();
        }

        //private static void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddLogging(configure => configure.AddConsole())
        //            .AddTransient<Parser>();
        //    services.AddLogging(configure => configure.AddConsole())
        //            .AddTransient<AggregatedRepository>();
        //}

    }

}
