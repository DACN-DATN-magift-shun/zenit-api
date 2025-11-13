using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Migrator.Seeds
{
    public class CategoryGroupSeed
    {
        public static readonly CategoryGroup[] groups = new[]
        {
            new CategoryGroup {
                Id = Guid.NewGuid(),
                Name = "Essential demand",
                GroupType = CategoryGroupType.Neccessary,
                CreatedAt = DateTime.UtcNow
            },
            new CategoryGroup {
                Id = Guid.NewGuid(),
                Name = "Savings",
                GroupType = CategoryGroupType.Savings,
                CreatedAt = DateTime.UtcNow
            },
            new CategoryGroup {
                Id = Guid.NewGuid(),
                Name = "Self Development",
                GroupType = CategoryGroupType.SelfDevelopment,
                CreatedAt = DateTime.UtcNow
            },
            new CategoryGroup {
                Id = Guid.NewGuid(),
                Name = "Entertainment",
                GroupType = CategoryGroupType.Entertainment,
                CreatedAt = DateTime.UtcNow
            },
            new CategoryGroup {
                Id = Guid.NewGuid(),
                Name = "Giving",
                GroupType = CategoryGroupType.Giving,
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}