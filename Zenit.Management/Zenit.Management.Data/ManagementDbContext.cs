using Microsoft.EntityFrameworkCore;

using Zenit.Share.Common.Constants;
using Zenit.Share.Data;
using Zenit.Share.Data.Extensions;

namespace Zenit.Management.Data
{
    public class ManagementDbContext : DbContextBase
    {
        public ManagementDbContext(DbContextOptions options) : base(options)
        {
            this.ConnectionString = GetConnectionString();
            this.MigrationAssembly = GetMigrationAssembly();
        }

        public static string GetConnectionString()
        {
            var connectionName = EnvConstants.MANAGEMENT_CONNECTION;

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
            return "Zenit.Management.Migrator";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("management");
            modelBuilder.RegisterAllEntities();
            base.OnModelCreating(modelBuilder);
        }
    }
}