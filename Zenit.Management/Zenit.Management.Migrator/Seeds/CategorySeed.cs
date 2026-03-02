using Zenit.Management.Common.Models;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Migrator.Seeds
{
    public class CategorySeed
    {
        public static readonly Category[] Categories = new[]
        {
                // Neccessary
                new Category {
                    Id = Guid.NewGuid(),
                    Name = "Shopping",
                    Icon = "shopping_cart_rounded",
                    Color = "#0D47A1",
                    BackgroundColor = "#E8F0FF",
                    GroupType = CategoryGroupType.Neccessary,
                    CreatedAt = DateTime.UtcNow
                },
                new Category {
                    Id = Guid.NewGuid(),
                    Name = "Eating",
                    Icon = "restaurant_rounded",
                    Color = "#B71C1C",
                    BackgroundColor = "#FFECEC",
                    GroupType = CategoryGroupType.Neccessary,
                    CreatedAt = DateTime.UtcNow
                },

                // Assets
                new Category {
                    Id = Guid.NewGuid(),
                    Name = "Savings",
                    Icon = "account_balance_rounded",
                    Color = "#1B5E20",
                    BackgroundColor = "#E8FBEA",
                    GroupType = CategoryGroupType.Assets,
                    CreatedAt = DateTime.UtcNow
                },
                new Category {
                    Id = Guid.NewGuid(),
                    Name = "Investing",
                    Icon = "trending_up_rounded",
                    Color = "#00695C",
                    BackgroundColor = "#E0F7F4",
                    GroupType = CategoryGroupType.Assets,
                    CreatedAt = DateTime.UtcNow
                },

                // SelfDevelopment
                new Category {
                    Id = Guid.NewGuid(),
                    Name = "Education",
                    Icon = "school_rounded",
                    Color = "#4A148C",
                    BackgroundColor = "#F3E6FA",
                    GroupType = CategoryGroupType.SelfDevelopment,
                    CreatedAt = DateTime.UtcNow
                },
                new Category {
                    Id = Guid.NewGuid(),
                    Name = "Reading",
                    Icon = "menu_book_rounded",
                    Color = "#283593",
                    BackgroundColor = "#EDEBFF",
                    GroupType = CategoryGroupType.SelfDevelopment,
                    CreatedAt = DateTime.UtcNow
                },

                // Entertainment
                new Category {
                    Id = Guid.NewGuid(),
                    Name = "Movies",
                    Icon = "movie_rounded",
                    Color = "#BF360C",
                    BackgroundColor = "#FFF2E6",
                    GroupType = CategoryGroupType.Entertainment,
                    CreatedAt = DateTime.UtcNow
                },
                new Category {
                    Id = Guid.NewGuid(),
                    Name = "Fitness",
                    Icon = "fitness_center_rounded",
                    Color = "#F57F17",
                    BackgroundColor = "#FFF8E1",
                    GroupType = CategoryGroupType.Entertainment,
                    CreatedAt = DateTime.UtcNow
                },

                // Income
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Salary",
                    Icon = "attach_money_rounded",
                    Color = "#0D47A1",
                    BackgroundColor = "#E8F0FF",
                    GroupType = CategoryGroupType.Income,
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Investing Returns",
                    Icon = "work_rounded",
                    Color = "#1B5E20",
                    BackgroundColor = "#E8FBEA",
                    GroupType = CategoryGroupType.Income,
                    CreatedAt = DateTime.UtcNow
                }
            };
    }
}