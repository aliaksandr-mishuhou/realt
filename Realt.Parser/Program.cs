//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

using Realt.Parser.DataAccess;

namespace Realt.Parser
{
    class Program
    {
        private const string PgSqlConnection = "Host=localhost;Username=realt;Password=;Database=realt";

        static void Main(string[] args)
        {
            IRunner runner = new Runner(new Parser(new DummyLogger<Parser>()),
                new AggregatedRepository(new IRepository[]
                {
                    new CsvRepository(),
                    new PgSqlRepository(PgSqlConnection, new DummyLogger<PgSqlRepository>()),
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
