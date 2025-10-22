using Microsoft.EntityFrameworkCore;

using Npgsql.Replication;

using Zenit.Share.Data.Interfaces;
using Zenit.Share.Migrator.Interfaces;

namespace Zenit.Share.Migrator.Stores;

public class EfSeederHistoryStore<TContext, THistory, TID>(
    TContext dbContext
) : ISeederHistoryStore
    where TContext : DbContext
    where THistory : class, ISeederHistory<TID>, new()
    where TID : struct
{
    public DbContext dbContext = dbContext;

    public async Task<HashSet<string>> GetExecutedSeedersAsync(CancellationToken cancellationToken = default)
    {
        var executedSeeders = await dbContext
            .Set<THistory>()
            .Select(x => x.SeederName)
            .ToListAsync(cancellationToken);

        return new HashSet<string>(executedSeeders, StringComparer.OrdinalIgnoreCase);
    }

    public async Task MarkSeederAsExecutedAsync(string seederName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(seederName))
        {
            throw new ArgumentNullException(nameof(seederName));
        }

        var seederHistory = new THistory
        {
            SeederName = seederName,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Set<THistory>().Add(seederHistory);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            // Handle potential concurrency issues or duplicates
            throw new Exception($"Failed to mark seeder '{seederName}' as executed.", ex);
        }
    }
}