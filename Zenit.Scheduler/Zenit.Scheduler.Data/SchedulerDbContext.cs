using Microsoft.EntityFrameworkCore;

using Zenit.Share.Data;
using Zenit.Share.Data.Extensions;

namespace Zenit.Scheduler.Data
{
    public class SchedulerDbContext : DbContextBase
    {
        public SchedulerDbContext(DbContextOptions options)
            : base(options)
        {
            this.ConnectionString = GetConnectionString();
            this.MigrationAssembly = GetMigrationAssembly();
        }

        private string GetConnectionString()
        {
            var connectionName = Environment.GetEnvironmentVariable("SCHEDULER_CONNECTION");

            if (string.IsNullOrEmpty(connectionName))
            {
                throw new InvalidOperationException($"Connection Name is not set.");
            }

            var connectionString = Environment.GetEnvironmentVariable(connectionName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string for '{connectionName}' is not set.");
            }

            return connectionString;
        }

        private string GetMigrationAssembly()
        {
            return "Zenit.Scheduler.Migrator";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RegisterAllEntities();
            base.OnModelCreating(modelBuilder);
        }
    }
}