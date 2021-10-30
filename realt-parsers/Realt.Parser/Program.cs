//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Realt.Parser.DataAccess;
using Realt.Parser.Onliner;
using Realt.Parser.Realt;

namespace Realt.Parser
{
    [Command(Name = "Parser")]
    [HelpOption("-?")]
    public class Program
    {
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Option("-s|--source", Description = "Source: " + SourceRealtValue + ", " + SourceOnlinerValue)]
        private string Source { get; set; }

        [Option("-v|--version", Description = "Version: " + V1Value + ", " + V2Value)]
        private string Version { get; set; }

        [Option("-t|--table", Description = "DB table name")]
        private string Table { get; set; }

        private static IConfigurationRoot ConfigurationRoot;

        private const string SourceOnlinerValue = "onliner";
        private const string SourceRealtValue = "realt";
        private const string V1Value = "1";
        private const string V2Value = "2";

#pragma warning disable IDE0051 // Remove unused private members
        private void OnExecute()
#pragma warning restore IDE0051 // Remove unused private members
        {
            InitConfiguration();

            var config = ConfigurationRoot;

            InitInputParams(config);

            Console.WriteLine($"{Source}, {Version}, {Table}");

            // parser
            (IParserV1 parserV1, IParserV2 parserV2) = InitParser(Source);

            var connectionString = ConfigurationRoot.GetConnectionString("history");

            IRunner runner = new Runner(
                parserV1,
                parserV2,
                new AggregatedRepository(new IRepository[]
                {
                    //new CsvRepository(),
                    new PgSqlRepository(connectionString, new DummyLogger<PgSqlRepository>(), Table),
                }, new DummyLogger<AggregatedRepository>()),
                new DummyLogger<Runner>());

            // version
            if (Version == V2Value)
            {
                runner.RunV2Async().Wait();
                return;
            }

            runner.RunV1Async().Wait();
        }

        private void InitInputParams(IConfigurationRoot config)
        {
            if (string.IsNullOrEmpty(Source))
            {
                Source = config.GetValue<string>("Settings:Source");
            }
            if (string.IsNullOrEmpty(Version))
            {
                Version = config.GetValue<string>("Settings:Version");
            }
            if (string.IsNullOrEmpty(Table))
            {
                Table = config.GetValue<string>("Settings:Table");
            }
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
