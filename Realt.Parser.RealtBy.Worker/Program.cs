using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Realt.Parser.Core;
using Realt.Parser.Repository.Rest;
using Serilog;

namespace Realt.Parser.RealtBy.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseSerilog()
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHostedService<DefaultWorker>();

                    // LEGACY APPROACH
                    //services.AddSingleton<IRepository, PgSqlRepository>(
                    //    options => new PgSqlRepository(
                    //        ctx.Configuration.GetValue<string>("DbConnection"),
                    //        options.GetService<ILogger<PgSqlRepository>>(),
                    //        ctx.Configuration.GetValue<string>("DbTable")
                    //        ));

                    services.AddSingleton<IRepository, RestRepository>(
                        options => new RestRepository(
                            ctx.Configuration.GetValue<string>("RepositoryServiceUrl"),
                            options.GetService<ILogger<RestRepository>>()
                            ));

                    services.AddSingleton<IParser, RealtByParser>(
                        options => new RealtByParser(
                            options.GetService<ILogger<RealtByParser>>(),
                            ctx.Configuration.GetValue<string>("ParamsConfigPath")
                            ));

                    services.AddSingleton<IRunner, Runner>();

                }).ConfigureLogging((ctx, logging) =>
                {
                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(ctx.Configuration)
                        .CreateLogger();

                    logging.AddSerilog(logger);
                });
    }
}
