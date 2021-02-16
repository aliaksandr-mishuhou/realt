//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Configuration;
using Realt.Parser.DataAccess;

namespace Realt.Parser
{
    class Program
    {
        private static IConfigurationRoot ConfigurationRoot;

        static void Main(string[] args)
        {
            InitConfiguration();

            var connectionString = ConfigurationRoot.GetConnectionString("history");

            IRunner runner = new Runner(new Parser(new DummyLogger<Parser>()),
                new AggregatedRepository(new IRepository[]
                {
                    new CsvRepository(),
                    new PgSqlRepository(connectionString, new DummyLogger<PgSqlRepository>()),
                }, new DummyLogger<AggregatedRepository>()),
                new DummyLogger<Runner>());
            runner.RunAsync().Wait();
        }

        static void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                //.AddJsonFile($"appsettings.Development.json", true, true)
                .AddEnvironmentVariables();

            ConfigurationRoot = builder.Build();
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
