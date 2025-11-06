using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Zenit.Share.Migrator;

public class DbStartup<TContext> : IHostedService
    where TContext : DbContext
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}