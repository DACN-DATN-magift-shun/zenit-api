using DotNetEnv;

using Zenit.Accounts.Data;
using Zenit.Share.Migrator;

namespace Zenit.Accounts.Migrator;
public class Program
{
    public static async Task Main(string[] args)
    {
        var force = true;
        var host = CreateHostBuilder(args).Build();
        await DbMigrator<AccountDbContext>.Run(host, force);
    }

    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddHostedService<DbStartup<AccountDbContext>>();
        services.AddDbContext<AccountDbContext>();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Env.Load();
        
        return Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureServices(ConfigureServices);
    }
        
}