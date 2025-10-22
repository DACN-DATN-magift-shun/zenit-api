namespace Zenit.Share.Migrator.Interfaces;

public interface ISeederHistoryStore
{
    Task<HashSet<string>> GetExecutedSeedersAsync(CancellationToken cancellationToken = default);

    Task MarkSeederAsExecutedAsync(string seederName, CancellationToken cancellationToken);
}