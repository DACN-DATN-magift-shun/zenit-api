using Microsoft.EntityFrameworkCore;

using Zenit.Share.Common.Constants;
using Zenit.Share.Data;
using Zenit.Share.Data.Extensions;

namespace Zenit.Statistics.Data
{
    public class StatisticsDbContext : DbContextBase
    {
        public StatisticsDbContext(DbContextOptions options) : base(options)
        {
            ConnectionString = GetConnectionString();
            MigrationAssembly = GetMigrationAssembly();
        }

        public static string GetConnectionString()
        {
            var connectionName = EnvConstants.STATISTICS_CONNECTION;

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

        public static string GetMigrationAssembly()
        {
            return "Zenit.Statistics.Migrator";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RegisterAllEntities();
            base.OnModelCreating(modelBuilder);
        }
    }
}