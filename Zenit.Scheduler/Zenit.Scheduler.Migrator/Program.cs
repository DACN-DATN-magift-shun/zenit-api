using SlnHost = Microsoft.Extensions.Hosting.Host;
using DotNetEnv;
using Zenit.Share.Migrator;
using Zenit.Scheduler.Data;

namespace Zenit.Scheduler.Migrator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var force = true;
            var host = CreateHostBuilder(args).Build();
            await DbMigrator<SchedulerDbContext>.Run(host, force);
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHostedService<DbStartup<SchedulerDbContext>>();
            services.AddDbContext<SchedulerDbContext>();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Env.Load();
            return SlnHost.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureServices(ConfigureServices);
        }
    }
}