using Microsoft.EntityFrameworkCore;

using Zenit.Share.Data.Extensions;

namespace Zenit.Share.Data
{
    public abstract class DbContextBase(DbContextOptions options) : DbContext(options)
    {
        public string? ConnectionString { get; set; }
        public string? MigrationAssembly { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            optionsBuilder.UseNpgsql(
                this.ConnectionString,
                optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(this.MigrationAssembly);
                }
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RegisterAllEntities();
            base.OnModelCreating(modelBuilder);
        }
    }
}