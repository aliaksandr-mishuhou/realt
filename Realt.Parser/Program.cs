//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

using System;
using Microsoft.Extensions.Configuration;
using Realt.Parser.DataAccess;
using Realt.Parser.Onliner;
using Realt.Parser.Realt;

namespace Realt.Parser
{
    class Program
    {
        private static IConfigurationRoot ConfigurationRoot;

        private const string SourceOnlinerValue = "onliner";
        private const string SourceRealtValue = "realt";
        private const string V1Value = "1";
        private const string V2Value = "2";

        static void Main(string[] args)
        {
            InitConfiguration();
            var config = ConfigurationRoot;
            var source = config.GetValue<string>("Settings:Source");
            var version = config.GetValue<string>("Settings:Version");
            var table = config.GetValue<string>("Settings:Table");

            // parser
            (IParserV1 parserV1, IParserV2 parserV2) = InitParser(source);

            var connectionString = ConfigurationRoot.GetConnectionString("history");

            IRunner runner = new Runner(
                parserV1,
                parserV2,
                new AggregatedRepository(new IRepository[]
                {
                    //new CsvRepository(),
                    new PgSqlRepository(connectionString, new DummyLogger<PgSqlRepository>(), table),
                }, new DummyLogger<AggregatedRepository>()),
                new DummyLogger<Runner>());

            // version
            if (version == V2Value)
            {
                runner.RunV2Async().Wait();
                return;
            }

            runner.RunV1Async().Wait();
        }

        private static (IParserV1 parserV1, IParserV2 parserV2) InitParser(string source)
        {
            if (source == SourceOnlinerValue)
            {
                var parser = new OnlinerParser(new DummyLogger<OnlinerParser>());
                return (parser, parser);
            }

            if (source == SourceRealtValue)
            {
                var parser = new RealtParser(new DummyLogger<RealtParser>());
                return (parser, parser);
            }

            throw new ArgumentException(nameof(source));
        }

        static void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
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
