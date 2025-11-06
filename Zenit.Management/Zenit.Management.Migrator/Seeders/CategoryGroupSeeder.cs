using Zenit.Management.Data;
using Zenit.Management.Data.Entities;
using Zenit.Management.Migrator.Seeds;
using Zenit.Share.Migrator.Interfaces;

namespace Zenit.Management.Migrator.Seeders
{
    public class CategoryGroupSeeder(ManagementDbContext dbContext) : IOrderedSeeder
    {
        public int Order => 1;

        public async Task Seed()
        {
            var categoryGroupTable = dbContext.Set<CategoryGroup>();

            var categoryGroups = CategoryGroupSeed.groups;

            categoryGroupTable.AddRange(categoryGroups);
            await dbContext.SaveChangesAsync();
        }
    }
}