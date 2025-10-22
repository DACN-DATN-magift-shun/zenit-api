using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Zenit.Share.Data.Interfaces;
using Zenit.Share.Migrator.Interfaces;
using Zenit.Share.Migrator.Stores;

namespace Zenit.Share.Migrator;

public static class DbSeeder
{
    public static async Task Run<TContext, THistory, TID>(
        IHost host
    )
        where TContext : DbContext
        where THistory : class, ISeederHistory<TID>, new()
        where TID : struct
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<TContext>();

        // get all executed seeders using history store
        var historyStore = new EfSeederHistoryStore<TContext, THistory, TID>(dbContext);

        var executedSeeders = new HashSet<string>(
            await historyStore.GetExecutedSeedersAsync(),
            StringComparer.OrdinalIgnoreCase
        );

        // get all assemblies implementing ISeeder
        var assembly = Assembly.GetEntryAssembly() ?? throw new Exception("Failed to get entry assembly");

        var seederTypes = assembly.GetTypes()
            .Where(t => typeof(ISeeder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

        if (seederTypes.Count == 0)
        {
            Console.WriteLine("No seeders found.");
            return;
        }

        // create all seeder instances
        var seederInstances = seederTypes
            .Select(t =>
            {
                if (ActivatorUtilities.CreateInstance(services, t) is not ISeeder instance)
                {
                    throw new Exception($"Failed to create seeder of type {t.FullName}");
                }

                var order = (instance is IOrderedSeeder orderedSeeder) ? orderedSeeder.Order : int.MaxValue;

                return new
                {
                    Type = t,
                    Name = t.FullName ?? t.Name,
                    Instance = instance,
                    Order = order,
                };
            })
            .OrderBy(s => s.Order)
            .ToList();

        // run seeders not executed yet and mark them as executed
        foreach (var seeder in seederInstances)
        {
            if (executedSeeders.Contains(seeder.Name))
            {
                Console.WriteLine($"Seeder {seeder.Name} already executed. Skipping.");
                continue;
            }

            Console.WriteLine($"Executing seeder {seeder.Name}...");

            try
            {
                await seeder.Instance.Seed();
                await historyStore.MarkSeederAsExecutedAsync(seeder.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to execute seeder {seeder.Name}: {ex.Message}");
                throw;
            }
        }
    }
}