using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Realt.Parser.Core;
//using Realt.Parser.Repository.Db;
using Realt.Parser.Repository.Rest;

namespace Realt.Parser.OnlinerBy.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHostedService<DefaultWorker>();

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
                    services.AddSingleton<IParser, OnlinerByParser>();
                    services.AddSingleton<IRunner, Runner>();
                });
    }
}
