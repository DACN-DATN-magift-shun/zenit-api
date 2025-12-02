using Zenit.Management.Data;
using Zenit.Management.Data.Entities;
using Zenit.Management.Migrator.Seeds;
using Zenit.Share.Migrator.Interfaces;

namespace Zenit.Management.Migrator.Seeders
{
    public class CategorySeeder(ManagementDbContext dbContext) : IOrderedSeeder
    {
        public int Order => 1;

        public async Task Seed()
        {
            var categoryTable = dbContext.Set<Category>();

            var categories = CategorySeed.Categories;

            categoryTable.AddRange(categories);
            await dbContext.SaveChangesAsync();
        }
    }
}