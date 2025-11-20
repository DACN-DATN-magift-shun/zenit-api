using DotNetEnv;

using Zenit.Share.Migrator;
using Zenit.Statistics.Data;

namespace Zenit.Statistics.Migrator
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var force = true;
            var host = CreateHostBuilder(args).Build();
            await DbMigrator<StatisticsDbContext>.Run(host, force);
            return 0;
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHostedService<DbStartup<StatisticsDbContext>>();
            services.AddDbContext<StatisticsDbContext>();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Env.Load();
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureServices(ConfigureServices);

        }
    }
}