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

        public string GetConnectionString()
        {
            var connectionName = EnvConstants.STATISTICS_CONNECTION ?? throw new Exception("Connection Name is not set.");
            Console.WriteLine($"Using connection name: {connectionName}");
            var connectionString = Environment.GetEnvironmentVariable(connectionName) ?? throw new Exception("Connection String is not set.");
            return connectionString;
        }

        public string GetMigrationAssembly()
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