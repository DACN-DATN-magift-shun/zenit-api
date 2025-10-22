using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Zenit.Share.Migrator;
public class DbMigrator<TContext>
    where TContext : DbContext
{
    public static async Task Run(IHost host, bool force = false)
    {
        await host.StartAsync();
        
        var context = host.Services.GetRequiredService<TContext>();
        Console.WriteLine("Applying migrations...");
        await context.Database.MigrateAsync();
    }
}
