using DotNetEnv;

using Zenit.Management.Data;
using Zenit.Management.Data.Entities;
using Zenit.Share.Migrator;

namespace Zenit.Management.Migrator
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var force = true;
            var host = CreateHostBuilder(args).Build();
            await DbMigrator<ManagementDbContext>.Run(host, force);
            await DbSeeder.Run<ManagementDbContext, ManagementSeederHistory, Guid>(host);
            return 0;
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHostedService<DbStartup<ManagementDbContext>>();
            services.AddDbContext<ManagementDbContext>();
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